using Microsoft.AspNetCore.Mvc;
using ShopSphere.BLL;

namespace ShopSphere.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? q)
        {
            var categories = await _productService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            ViewBag.CurrentCategoryId = categoryId;
            ViewBag.SearchQuery = q ?? string.Empty;

            IEnumerable<ShopSphere.Domain.Models.Product> products;

            if (!string.IsNullOrWhiteSpace(q))
            {
                products = await _productService.SearchProductsAsync(q.Trim());
            }
            else if (categoryId.HasValue)
            {
                products = await _productService.GetProductsByCategoryAsync(categoryId.Value);
            }
            else
            {
                products = await _productService.GetApprovedProductsAsync();
            }

            return View(products);
        }


        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null || product.Status != "Approved")
                return NotFound();

            return View(product);
        }

    }
}
