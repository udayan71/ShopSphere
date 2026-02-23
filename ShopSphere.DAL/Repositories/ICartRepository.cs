using ShopSphere.Domain.Models;
namespace ShopSphere.DAL.Repositories
{
    public interface ICartRepository
    {
        Task AddToCartAsync(string userId, int productId, int quantity);

        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task RemoveCartItemAsync(int cartItemId);
        Task UpdateQuantityAsync(int cartItemId, int quantity);
        Task ClearCartAsync(string userId);

    }

}
