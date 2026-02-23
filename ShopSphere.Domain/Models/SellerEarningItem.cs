

namespace ShopSphere.Domain.Models
{
    public class SellerEarningItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? CommissionAmount { get; set; }
        public decimal? SellerAmount { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
