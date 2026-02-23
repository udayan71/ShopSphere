
namespace ShopSphere.Domain.Models
{
    public class SellerEarningsSummary
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalEarnings { get; set; }
    }
}
