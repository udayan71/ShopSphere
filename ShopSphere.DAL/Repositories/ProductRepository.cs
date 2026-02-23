using ShopSphere.Domain.Models;
using ShopSphere.DAL.Repositories;
using ShopSphere.DAL.Context;
using Dapper;
using System.Data;

namespace ShopSphere.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "InsertProduct");
            parameters.Add("@SellerId", product.SellerId);
            parameters.Add("@Name", product.Name);
            parameters.Add("@Description", product.Description);
            parameters.Add("@Price", product.Price);
            parameters.Add("@Stock", product.Stock);
            using var connection = _context.CreateConnection();

            var productId=await connection.ExecuteScalarAsync<int>("sp_Product_CRUD",
                parameters, commandType: CommandType.StoredProcedure);
            return productId;
        }
        public async Task ReduceStockAsync(int productId, int quantity)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "ReduceStock");
            parameters.Add("@ProductId", productId);
            parameters.Add("@Quantity", quantity);

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_Product_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetSellerProductsAsync(int sellerId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetSellerProducts");
            parameters.Add("@SellerId", sellerId);
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(
                "sp_Product_CRUD", parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetPendingProductsAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetPendingProducts");
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(
                "sp_Product_CRUD", parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task ApproveProductAsync(int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "ApproveProduct");
            parameters.Add("@ProductId", productId);
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("sp_Product_CRUD",
                parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task RejectProductAsync(int productId, string reason)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "RejectProduct");
            parameters.Add("@ProductId", productId);
            parameters.Add("@RejectionReason", reason);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Product_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        public async Task AddProductImageAsync(int productId, string imagePath)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "InsertProductImage");
            parameters.Add("@ProductId", productId);
            parameters.Add("@ImagePath", imagePath);

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(
                "sp_Product_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesAsync(int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetProductImages");
            parameters.Add("@ProductId", productId);

            using var connection = _context.CreateConnection();

            return await connection.QueryAsync<ProductImage>(
                "sp_Product_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetProductById");
            parameters.Add("@ProductId", productId);

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Product>(
                "sp_Product_CRUD",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Product>> GetApprovedProductsAsync()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Flag", "GetApprovedProducts");
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Product>(
                "sp_Product_CRUD", parameters,
                commandType: CommandType.StoredProcedure);
        }

    }
}
