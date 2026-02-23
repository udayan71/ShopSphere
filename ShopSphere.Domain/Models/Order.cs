

using ShopSphere.Domain.Enums;

namespace ShopSphere.Domain.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

}
