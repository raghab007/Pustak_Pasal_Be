using AlishPustakGhar.Model;
using AlishPustakGhar.Models;

namespace AlishPustakGhar.Services.Interfaces;

public interface ICartService
{
    Task<Cart> GetUserCartAsync(Guid userId);
    Task AddToCartAsync(Guid userId, Guid bookId, int quantity);
    Task UpdateCartItemAsync(Guid userId, Guid bookId, int quantity);
    Task RemoveFromCartAsync(Guid userId, Guid bookId);
    Task ClearCartAsync(Guid userId);
    
    Task<List<CartItem>> GetCartItems(Guid userId);

}