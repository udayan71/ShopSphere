using ShopSphere.DAL.Repositories;
using ShopSphere.Domain.Models;


namespace ShopSphere.BLL
{

    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be at least 1.");

            await _repository.AddToCartAsync(userId, productId, quantity);
        }

        public async Task ClearCartAsync(string userId)
        {
            await _repository.ClearCartAsync(userId);
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be at least 1.");

            await _repository.UpdateQuantityAsync(cartItemId, quantity);
        }


        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            return await _repository.GetCartItemsAsync(userId);
        }

        public async Task RemoveCartItemAsync(int cartItemId)
        {
            await _repository.RemoveCartItemAsync(cartItemId);
        }

        public async Task<decimal> GetCartTotalAsync(string userId)
        {
            var items = await _repository.GetCartItemsAsync(userId);

            decimal total = 0;

            foreach (var item in items)
            {
                total += item.Price * item.Quantity;
            }

            return total;
        }
    }

}
