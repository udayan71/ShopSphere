

namespace ShopSphere.Domain.Models
{
    public class ProductImage
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedDate { get; set; }


    }
}
