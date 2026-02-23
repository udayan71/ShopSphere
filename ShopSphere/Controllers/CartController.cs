using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.BLL;
using ShopSphere.Identity;

namespace ShopSphere.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService,
                              UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var items = await _cartService.GetCartItemsAsync(user.Id);
            ViewBag.Total = await _cartService.GetCartTotalAsync(user.Id);

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);

            await _cartService.AddToCartAsync(user.Id, productId, quantity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, quantity);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveCartItemAsync(id);
            return RedirectToAction("Index");
        }
    }


}
