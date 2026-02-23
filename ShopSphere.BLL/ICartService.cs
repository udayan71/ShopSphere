using ShopSphere.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.BLL
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId, int quantity);
        Task UpdateQuantityAsync(int cartItemId, int quantity);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task RemoveCartItemAsync(int cartItemId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task ClearCartAsync(string userId);

    }

}
