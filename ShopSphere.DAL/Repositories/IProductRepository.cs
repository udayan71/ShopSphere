using ShopSphere.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.DAL.Repositories
{
    public interface IProductRepository
    {
        Task<int> AddProductAsync(Product product);
        Task<IEnumerable<Product>> GetSellerProductsAsync(int sellerId);
        Task<IEnumerable<Product>> GetPendingProductsAsync();
        Task ApproveProductAsync(int productId);
        Task RejectProductAsync(int productId,string reason);
        Task AddProductImageAsync(int productId, string imagePath);
        Task<IEnumerable<ProductImage>> GetProductImagesAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task ReduceStockAsync(int productId, int quantity);


        Task<IEnumerable<Product>> GetApprovedProductsAsync();


        Task <IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoriesAsync();
    }
}
