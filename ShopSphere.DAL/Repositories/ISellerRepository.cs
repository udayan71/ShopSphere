using ShopSphere.Domain.Models;

public interface ISellerRepository
{
    Task AddSellerAsync(Seller seller);
    Task<Seller?> GetSellerByUserIdAsync(string userId);
    Task<IEnumerable<Seller>> GetPendingSellersAsync();
    Task ApproveSellerAsync(int sellerId);
    Task RejectSellerAsync(int sellerId, string reason);
    Task ReapplySellerAsync(Seller seller);
    Task<bool> SellerExistsAsync(string userId);
    Task<Seller?> GetSellerByIdAsync(int sellerId);


}
