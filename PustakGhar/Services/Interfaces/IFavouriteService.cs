// IFavouritesService.cs
using AlishPustakGhar.Models;
using System;
using System.Threading.Tasks;

namespace AlishPustakGhar.Services
{
    public interface IFavouritesService
    {
        Task<bool> AddToFavouritesAsync(Guid userId, Guid bookId);
        Task<bool> RemoveFromFavouritesAsync(Guid userId, Guid bookId);
        Task<bool> IsBookInFavouritesAsync(Guid userId, Guid bookId);
    }
}