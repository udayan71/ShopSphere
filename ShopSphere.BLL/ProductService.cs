using ShopSphere.Domain.Models;
using ShopSphere.DAL.Repositories;

namespace ShopSphere.BLL
{
    public class ProductService : IProductService
    {
        public readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            return await _productRepository.AddProductAsync(product);
        }

        public async Task<IEnumerable<Product>> GetSellerProductsAsync(int sellerId)
        {
            var products = (await _productRepository
                            .GetSellerProductsAsync(sellerId))
                            .ToList();

            foreach (var product in products)
            {
                var images = await _productRepository
                                   .GetProductImagesAsync(product.ProductId);

                product.Images = images.ToList();
            }

            return products;
        }


        public async Task<IEnumerable<Product>> GetPendingProductsAsync()
        {
            var products = (await _productRepository.GetPendingProductsAsync()).ToList();

            foreach (var product in products)
            {
                var images = await _productRepository.GetProductImagesAsync(product.ProductId);
                product.Images = images.ToList();
            }

            return products;
        }


        public async Task ApproveProductAsync(int productId)
        {
            await _productRepository.ApproveProductAsync(productId);
        }

        public async Task RejectProductAsync(int productId,string reason)
        {
            await _productRepository.RejectProductAsync(productId, reason);
        }
        public async Task AddProductImageAsync(int productId, string imagePath)
        {
            await _productRepository.AddProductImageAsync(productId, imagePath);
        }
        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);

            if (product != null)
            {
                var images = await _productRepository.GetProductImagesAsync(productId);
                product.Images = images.ToList();
            }

            return product;
        }

        public async Task ReduceStockAsync(int productId, int quantity)
        {
            await _productRepository.ReduceStockAsync(productId, quantity);
        }

        public async Task<IEnumerable<Product>> GetApprovedProductsAsync()
        {
            var products = (await _productRepository
                            .GetApprovedProductsAsync())
                            .ToList();

            foreach (var product in products)
            {
                var images = await _productRepository
                                   .GetProductImagesAsync(product.ProductId);

                product.Images = images.ToList();
            }

            return products;
        }


    }
}
