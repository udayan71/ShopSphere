namespace ShopSphere.Domain.Models
{
    public class Seller
    {
        public int SellerId { get; set; }
        public string UserId { get; set; }
        public string BusinessName { get; set; }

        public string Status { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string GSTNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? RejectionReason { get; set; }

        public DateTime? RejectedDate { get; set; }
    }
}
