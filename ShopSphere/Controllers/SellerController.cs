using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopSphere;
using ShopSphere.BLL;
using ShopSphere.Domain.Enums;
using ShopSphere.Domain.Models;
using ShopSphere.Identity;


namespace ShopSphere.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private readonly ISellerService _sellerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public SellerController(
            ISellerService sellerService,
            UserManager<ApplicationUser> userManager,
            IProductService productService,
            IOrderService orderService)
        {
            _sellerService = sellerService;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult BecomeSeller()
        {
            return View();
        }

        public async Task<IActionResult> Earnings()
        {
            var user = await _userManager.GetUserAsync(User);
            var seller = await _sellerService.GetSellerByUserIdAsync(user.Id);

            if (seller == null)
                return RedirectToAction("Dashboard");

            var summary = await _orderService.GetSellerEarningsSummaryAsync(seller.SellerId);
            var details = await _orderService.GetSellerEarningsDetailsAsync(seller.SellerId);

            var model = new SellerEarningsDashboardViewModel
            {
                Summary = summary,
                Earnings = details.ToList()
            };

            return View(model);
        }


        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            var seller = await _sellerService.GetSellerByUserIdAsync(user.Id);

            if (seller == null || seller.Status != "Approved")
                return RedirectToAction("Apply");

            var products = await _productService.GetSellerProductsAsync(seller.SellerId);

            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            // load categories and pass a SelectList (not the Task)
            var categories = (await _productService.GetCategoriesAsync()).ToList();
            ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model, List<IFormFile>? images)
        {
            if (!ModelState.IsValid)
            {
                // repopulate the SelectList when returning the view on validation error
                var cats = (await _productService.GetCategoriesAsync()).ToList();
                ViewBag.Categories = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(cats, "CategoryId", "Name");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var seller = await _sellerService.GetSellerByUserIdAsync(user.Id);
            if (seller == null || seller.Status != "Approved")
                return RedirectToAction("Dashboard");

            model.SellerId = seller.SellerId;

            // IMPORTANT: Always new product should be Pending
            model.Status = "Pending";

            var productId = await _productService.AddProductAsync(model);

            if (images != null && images.Any())
            {
                var uploadRoot = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "products");

                var productFolder = Path.Combine(uploadRoot, productId.ToString());

                if (!Directory.Exists(productFolder))
                {
                    Directory.CreateDirectory(productFolder);
                }

                foreach (var image in images)
                {
                    if (image.Length > 0)
                    {
                        var extension = Path.GetExtension(image.FileName).ToLower();

                        // Basic image validation
                        if (extension != ".jpg" &&
                            extension != ".jpeg" &&
                            extension != ".png")
                            continue;

                        var fileName = Guid.NewGuid() + extension;

                        var filePath = Path.Combine(productFolder, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await image.CopyToAsync(stream);

                        var dbPath = $"/uploads/products/{productId}/{fileName}";

                        await _productService.AddProductImageAsync(productId, dbPath);
                    }
                }
            }

            return RedirectToAction("Dashboard");
        }

        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var seller = await _sellerService.GetSellerByUserIdAsync(user.Id);
            if (seller == null || seller.Status != "Approved")
                return RedirectToAction("Dashboard");

            var orders = await _orderService.GetOrdersBySellerAsync(seller.SellerId);

            return View(orders);
        }
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> MarkReady(int orderItemId)
        {
            await _orderService.UpdateOrderItemStatusAsync(
                orderItemId,
                OrderStatus.ReadyForPickup);

            return RedirectToAction("Orders");
        }


        [HttpPost]
        public async Task<IActionResult> BecomeSeller(BecomeSellerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            try
            {
                await _sellerService.RegisterSellerAsync(
                    user.Id,
                    model.BusinessName,
                    model.PhoneNumber,
                    model.Address,
                    model.GSTNumber);

                TempData["SuccessMessage"] = "Application submitted successfully.";

                return RedirectToAction("BecomeSeller");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }



        public IActionResult SellerRequestSubmitted()
        {
            return View();
        }
    }
}