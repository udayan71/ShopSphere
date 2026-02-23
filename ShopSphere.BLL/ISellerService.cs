using ShopSphere.Domain.Models;

public interface ISellerService
{
    Task RegisterSellerAsync(string userId, string businessName,string phone,string address,string gst);
    Task<IEnumerable<Seller>> GetPendingSellersAsync();
    Task RejectSellerAsync(int sellerId,string reason);
    Task ApproveSellerAsync(int sellerId);
    Task<bool> SellerExistsAsync(string userId);
    Task<Seller?> GetSellerByUserIdAsync(string userId);
    Task<Seller?> GetSellerByIdAsync(int sellerId);


}
