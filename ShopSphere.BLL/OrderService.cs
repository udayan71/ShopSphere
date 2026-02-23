using ShopSphere.DAL.Repositories;
using ShopSphere.Domain.Enums;
using ShopSphere.Domain.Models;

namespace ShopSphere.BLL
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepository,
                            ICartService cartService,
                            IProductService productService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<int> PlaceOrderAsync(string userId, string paymentMethod)
        {
            var cartItems = await _cartService.GetCartItemsAsync(userId);

            decimal total = cartItems.Sum(x => x.Price * x.Quantity);

            var order = new Order
            {
                UserId = userId,
                TotalAmount = total,
                PaymentMethod = paymentMethod,
                PaymentStatus = paymentMethod == "Online" ? "Paid" : "Pending",
                Status = OrderStatus.Pending
            };

            var orderId = await _orderRepository.CreateOrderAsync(order);

            foreach (var item in cartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);

                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SellerId = product.SellerId
                };

                await _orderRepository.AddOrderItemAsync(orderItem);

                await _productService.ReduceStockAsync(item.ProductId, item.Quantity);
            }

            await _cartService.ClearCartAsync(userId);

            return orderId;
        }


        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersBySellerAsync(int sellerId)
        {
            return await _orderRepository.GetOrdersBySellerAsync(sellerId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            await _orderRepository.UpdateOrderStatusAsync(orderId, (int)status);
        }

        public async Task UpdateOrderItemStatusAsync(int orderItemId, OrderStatus status)
        {
            await _orderRepository.UpdateOrderItemStatusAsync(orderItemId, (int)status);
        }
        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _orderRepository.GetAllOrderItemsAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersByUserAsync(string userId)
        {
            var allItems = await _orderRepository.GetAllOrderItemsAsync();
            return allItems.Where(x => x.UserId == userId);
        }

        public async Task<SellerEarningsSummary> GetSellerEarningsSummaryAsync(int sellerId)
        {
            return await _orderRepository.GetSellerEarningsSummaryAsync(sellerId);
        }

        public async Task<IEnumerable<SellerEarningItem>> GetSellerEarningsDetailsAsync(int sellerId)
        {
            return await _orderRepository.GetSellerEarningsDetailsAsync(sellerId);
        }



        public async Task<decimal> GetTotalRevenueAsync()
    => await _orderRepository.GetTotalRevenueAsync();

        public async Task<decimal> GetTotalCommissionAsync()
            => await _orderRepository.GetTotalCommissionAsync();

        public async Task<int> GetDeliveredOrdersCountAsync()
            => await _orderRepository.GetDeliveredOrdersCountAsync();

    }

}
