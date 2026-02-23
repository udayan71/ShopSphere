using ShopSphere.Domain.Models;

namespace ShopSphere.DAL.Repositories
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);

        Task AddOrderItemAsync(OrderItem item);

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<OrderItem>> GetOrdersBySellerAsync(int sellerId);

        Task UpdateOrderStatusAsync(int orderId, int status);
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task<IEnumerable<OrderItem>> GetOrdersByUserAsync(string userId);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetTotalCommissionAsync();
        Task<int> GetDeliveredOrdersCountAsync();
        Task<SellerEarningsSummary> GetSellerEarningsSummaryAsync(int sellerId);
        Task<IEnumerable<SellerEarningItem>> GetSellerEarningsDetailsAsync(int sellerId);


        Task UpdateOrderItemStatusAsync(int orderItemId, int status);
    }


}
