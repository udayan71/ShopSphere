using Dapper;
using ShopSphere.DAL.Context;
using ShopSphere.Domain.Models;
using System.Data;

namespace ShopSphere.DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;

        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "CreateOrder");
            parameters.Add("@UserId", order.UserId);
            parameters.Add("@TotalAmount", order.TotalAmount);
            parameters.Add("@PaymentMethod", order.PaymentMethod);
            parameters.Add("@PaymentStatus", order.PaymentStatus);

            using var connection = _context.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<SellerEarningsSummary> GetSellerEarningsSummaryAsync(int sellerId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetSellerEarningsSummary");
            parameters.Add("@SellerId", sellerId);

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<SellerEarningsSummary>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<SellerEarningItem>> GetSellerEarningsDetailsAsync(int sellerId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetSellerEarningsDetails");
            parameters.Add("@SellerId", sellerId);

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<SellerEarningItem>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task AddOrderItemAsync(OrderItem item)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "AddOrderItem");
            parameters.Add("@OrderId", item.OrderId);
            parameters.Add("@ProductId", item.ProductId);
            parameters.Add("@Quantity", item.Quantity);
            parameters.Add("@Price", item.Price);
            parameters.Add("@SellerId", item.SellerId);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetOrders");

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<Order>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OrderItem>> GetOrdersBySellerAsync(int sellerId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetOrdersBySeller");
            parameters.Add("@SellerId", sellerId);

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<OrderItem>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task UpdateOrderStatusAsync(int orderId, int status)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "UpdateOrderStatus");
            parameters.Add("@OrderId", orderId);
            parameters.Add("@Status", status);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateOrderItemStatusAsync(int orderItemId, int status)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "UpdateOrderItemStatus");
            parameters.Add("@OrderItemId", orderItemId);
            parameters.Add("@Status", status);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetAllOrderItems");
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<OrderItem>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<OrderItem>> GetOrdersByUserAsync(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetOrdersByUser");
            parameters.Add("@UserId", userId);

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<OrderItem>(
                "sp_Order_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(Price * Quantity),0) FROM OrderItems WHERE Status = 5");
        }

        public async Task<decimal> GetTotalCommissionAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(
                "SELECT ISNULL(SUM(CommissionAmount),0) FROM OrderItems WHERE Status = 5");
        }

        public async Task<int> GetDeliveredOrdersCountAsync()
        {
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM OrderItems WHERE Status = 5");
        }


    }

}
