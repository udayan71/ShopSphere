

namespace ShopSphere.Domain.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
    }

}
