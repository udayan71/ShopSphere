using Dapper;
using ShopSphere.DAL.Context;
using ShopSphere.Domain.Models;
using System.Data;

namespace ShopSphere.DAL.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly DapperContext _context;
        public CartRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "AddToCart");
            parameters.Add("@UserId", userId);
            parameters.Add("@ProductId", productId);
            parameters.Add("@Quantity", quantity);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Cart_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task ClearCartAsync(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "ClearCart");
            parameters.Add("@UserId", userId);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_Cart_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetCartItems");
            parameters.Add("@UserId", userId);

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CartItem>(
                "sp_Cart_CRUD", parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "UpdateQuantity");
            parameters.Add("@CartItemId", cartItemId);
            parameters.Add("@Quantity", quantity);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Cart_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }


        public async Task RemoveCartItemAsync(int cartItemId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "RemoveCartItem");
            parameters.Add("@CartItemId", cartItemId);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_Cart_CRUD",
                parameters, commandType: CommandType.StoredProcedure);
        }


    }
}
