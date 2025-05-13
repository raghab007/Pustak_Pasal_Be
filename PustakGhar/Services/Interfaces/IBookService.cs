using AlishPustakGhar.Dtos;

namespace AlishPustakGhar.Services.Interfaces;

public interface IBookService
{
    Task<string> AddBook(BookAddDto bookAddDto);
    
    
    BookResponseDto GetPaginatedBooks(int page, int pageSize);
    
    
    
    Task<BookWithDetailsDto> GetBookById(Guid id);
    //void UpdateBook(Guid id, )
    
    Task<bool> ApplyBulkDiscount(BulkDiscountDto bulkDiscountDto);
    
    Task<BookResponseDto> SearchBooksByTitle(string title, int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBooksByGenre(Guid genreId, int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBooksByGenreType(string genreType, int page = 1, int pageSize = 1);
    
    
    Task<bool> DeleteBook(Guid id);
    Task<bool> UpdateBook(Guid id, BookUpdateDto bookUpdateDto);
    Task<BookResponseDto> GetComingSoonBooks(int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBooksOnSale(int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBestSellers(int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetNewReleases(int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetNewArrivals(int page = 1, int pageSize = 12);
    
    Task<List<PurchasedBookDto>> GetPurchasedBooks(Guid userId);
}