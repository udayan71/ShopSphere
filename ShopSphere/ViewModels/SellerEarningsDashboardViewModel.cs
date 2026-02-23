using ShopSphere.Domain.Models;

public class SellerEarningsDashboardViewModel
{
    public SellerEarningsSummary Summary { get; set; }
    public List<SellerEarningItem> Earnings { get; set; }
}
