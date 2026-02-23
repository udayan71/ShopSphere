using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.BLL;
using ShopSphere.Domain.Enums;
using ShopSphere.ViewModels;
using ShopSphere.Identity;
using ClosedXML.Excel;
using System.IO;



namespace ShopSphere.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ISellerService _sellerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public AdminController(
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

        public async Task<IActionResult> ProductApprovals()
        {
            var pendingProducts = await _productService.GetPendingProductsAsync();
            return View(pendingProducts);
        }
        public async Task<IActionResult> ExportOrdersToExcel()
        {
            var orders = await _orderService.GetAllOrderItemsAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");

                // Header Row
                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Product ID";
                worksheet.Cell(1, 3).Value = "Quantity";
                worksheet.Cell(1, 4).Value = "Price";
                worksheet.Cell(1, 5).Value = "Seller ID";
                worksheet.Cell(1, 6).Value = "Status";
                

                int row = 2;

                foreach (var order in orders)
                {
                    worksheet.Cell(row, 1).Value = order.OrderId;
                    worksheet.Cell(row, 2).Value = order.ProductId;
                    worksheet.Cell(row, 3).Value = order.Quantity;
                    worksheet.Cell(row, 4).Value = order.Price;
                    worksheet.Cell(row, 5).Value = order.SellerId;
                    worksheet.Cell(row, 6).Value = order.Status.ToString();
                    

                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Orders_Report.xlsx");
                }
            }
        }

        public async Task<IActionResult> ApproveProduct(int productId)
        {
            await _productService.ApproveProductAsync(productId);
            return RedirectToAction("ProductApprovals");
        }
        [HttpPost]
        public async Task<IActionResult> RejectProduct(int productId, string reason)
        {
            await _productService.RejectProductAsync(productId, reason);
            return RedirectToAction("ProductApprovals");
        }

        public async Task<IActionResult> Index()
        {
            var pendingSellers = await _sellerService.GetPendingSellersAsync();
            return View(pendingSellers);
        }

        public async Task<IActionResult> ApproveSeller(int sellerId)
        {

            await _sellerService.ApproveSellerAsync(sellerId);


            var seller = await _sellerService.GetSellerByIdAsync(sellerId);

            if (seller == null)
                return RedirectToAction("Index");


            var user = await _userManager.FindByIdAsync(seller.UserId);

            if (user == null)
                return RedirectToAction("Index");


            if (!await _userManager.IsInRoleAsync(user, "Seller"))
            {
                await _userManager.AddToRoleAsync(user, "Seller");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> RejectSeller(int sellerId, string reason)
        {
            await _sellerService.RejectSellerAsync(sellerId, reason);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Orders()
        {
            var orders = await _orderService.GetAllOrderItemsAsync();
            return View(orders);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderItemId, int status)
        {
            await _orderService.UpdateOrderItemStatusAsync(
                orderItemId,
                (OrderStatus)status);

            return RedirectToAction("Orders");
        }
        public async Task<IActionResult> Dashboard()
        {
            var totalRevenue = await _orderService.GetTotalRevenueAsync();
            var totalCommission = await _orderService.GetTotalCommissionAsync();
            var deliveredOrders = await _orderService.GetDeliveredOrdersCountAsync();

            var model = new AdminDashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalCommission = totalCommission,
                DeliveredOrders = deliveredOrders
            };

            return View(model);
        }

    }
}