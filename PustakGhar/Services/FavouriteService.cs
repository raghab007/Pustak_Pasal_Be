// FavouritesService.cs
using AlishPustakGhar.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AlishPustakGhar.Data;

namespace AlishPustakGhar.Services
{
    public class FavouritesService : IFavouritesService
    {
        private readonly ApplicationDbContext _context;

        public FavouritesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToFavouritesAsync(Guid userId, Guid bookId)
        {
            // Check if the book is already in favourites
            var existingFavourite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.userId == userId && f.bookId == bookId);

            if (existingFavourite != null)
            {
                return false; // Already in favourites
            }

            // Check if user and book exist
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!userExists || !bookExists)
            {
                return false;
            }

            // Add to favourites
            var favourite = new Favourites
            {
                userId = userId,
                bookId = bookId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFromFavouritesAsync(Guid userId, Guid bookId)
        {
            var favourite = await _context.Favourites
                .FirstOrDefaultAsync(f => f.userId == userId && f.bookId == bookId);

            if (favourite == null)
            {
                return false;
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsBookInFavouritesAsync(Guid userId, Guid bookId)
        {
            return await _context.Favourites
                .AnyAsync(f => f.userId == userId && f.bookId == bookId);
        }
    }
}