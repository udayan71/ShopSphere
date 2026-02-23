using System.ComponentModel.DataAnnotations;

public class BecomeSellerViewModel
{
    [Required]
    public string BusinessName { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    [Display(Name = "GST Number")]
    public string GSTNumber { get; set; }

    public string? Status { get; set; }
    public string? RejectionReason { get; set; }
}
