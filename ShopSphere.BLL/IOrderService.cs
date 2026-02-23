using ShopSphere.Domain.Enums;
using ShopSphere.Domain.Models;


namespace ShopSphere.BLL
{
    public interface IOrderService
    {
        Task<int> PlaceOrderAsync(string userId, string paymentMethod);
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<IEnumerable<OrderItem>> GetOrdersByUserAsync(string userId);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalCommissionAsync();
        Task<int> GetDeliveredOrdersCountAsync();
        Task<SellerEarningsSummary> GetSellerEarningsSummaryAsync(int sellerId);
        Task<IEnumerable<SellerEarningItem>> GetSellerEarningsDetailsAsync(int sellerId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<OrderItem>> GetOrdersBySellerAsync(int sellerId);
        Task UpdateOrderItemStatusAsync(int orderItemId, OrderStatus status);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }

}
