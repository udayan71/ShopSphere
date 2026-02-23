

namespace ShopSphere.Domain.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }

        //Category
        public int? CategoryId { get; set; }
        public String? CategoryName { get; set; }



        public string Name { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        public string Status { get; set; } = "Pending";

        public bool IsApproved { get; set; }

        public string? RejectionReason { get; set; }
        public DateTime? RejectedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<ProductImage>? Images { get; set; }
    }

}
