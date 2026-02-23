using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.BLL;
using ShopSphere.Identity;

namespace ShopSphere.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IOrderService orderService,
                                  UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string paymentMethod)
        {
            var user = await _userManager.GetUserAsync(User);

            await _orderService.PlaceOrderAsync(user.Id, paymentMethod);

            return RedirectToAction("Index","Products");
        }

        public IActionResult Success()
        {
            return View();
        }
    }

}
