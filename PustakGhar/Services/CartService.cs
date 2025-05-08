using AlishPustakGhar.Data;
using AlishPustakGhar.Model;
using AlishPustakGhar.Models;
using AlishPustakGhar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlishPustakGhar.Services;

using System;
using System.Linq;
using System.Threading.Tasks;




public class CartService : ICartService
{
    private readonly ApplicationDbContext _context;

    public CartService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart> GetUserCartAsync(Guid userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartBooks)
            .ThenInclude(cb => cb.Book)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            // Create a new cart if one doesn't exist
            cart = new Cart
            {
                UserId = userId,
                CartBooks = new List<CartBook>()
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }

    public async Task AddToCartAsync(Guid userId, Guid bookId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }

        // Check if book exists and has sufficient stock
        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
        {
            throw new KeyNotFoundException("Book not found");
        }

        if (!book.IsAvailable || book.Stock < quantity)
        {
            throw new InvalidOperationException("Book is not available in the requested quantity");
        }

        var cart = await GetUserCartAsync(userId);

        // Check if book already exists in cart
        var existingCartItem = cart.CartBooks.FirstOrDefault(cb => cb.BookId == bookId);
        if (existingCartItem != null)
        {
            // Update existing item's quantity
            existingCartItem.Quantity += quantity;

            // Verify stock is still sufficient
            if (book.Stock < existingCartItem.Quantity)
            {
                throw new InvalidOperationException("Requested quantity exceeds available stock");
            }
        }
        else
        {
            // Add new item to cart
            var cartBook = new CartBook
            {
                BookId = bookId,
                Quantity = quantity,
                CartId = cart.Id
            };
            cart.CartBooks.Add(cartBook);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCartItemAsync(Guid userId, Guid bookId, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }

        var book = await _context.Books.FindAsync(bookId);
        if (book == null)
        {
            throw new KeyNotFoundException("Book not found");
        }

        if (!book.IsAvailable || book.Stock < quantity)
        {
            throw new InvalidOperationException("Book is not available in the requested quantity");
        }

        var cart = await GetUserCartAsync(userId);
        var cartItem = cart.CartBooks.FirstOrDefault(cb => cb.BookId == bookId);

        if (cartItem == null)
        {
            throw new KeyNotFoundException("Item not found in cart");
        }

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromCartAsync(Guid userId, Guid bookId)
    {
        var cart = await GetUserCartAsync(userId);
        var cartItem = cart.CartBooks.FirstOrDefault(cb => cb.BookId == bookId);

        if (cartItem != null)
        {
            cart.CartBooks.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearCartAsync(Guid userId)
    {
        var cart = await GetUserCartAsync(userId);
        cart.CartBooks.Clear();
        await _context.SaveChangesAsync();
    }
}
