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

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetApprovedProductsAsync();
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
