using ShopSphere.Domain.Models;

namespace ShopSphere.BLL
{
    public interface IProductService
    {
        Task<int> AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetSellerProductsAsync(int sellerId);
        Task<IEnumerable<Product>> GetPendingProductsAsync();
        Task ApproveProductAsync(int productId);
        Task RejectProductAsync(int productId,string reason);
        Task AddProductImageAsync(int productId, string imagePath);
        Task<Product?> GetProductByIdAsync(int productId);
        Task ReduceStockAsync(int productId, int quantity);

        Task<IEnumerable<Product>> GetApprovedProductsAsync();


        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}
