
using ShopSphere.Domain.Enums;

namespace ShopSphere.Domain.Models
{
    public class OrderItem
    {
        public string UserId { get; set; }
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int SellerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        public OrderStatus Status { get; set; }
    }

}
