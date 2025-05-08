using AlishPustakGhar.Dtos;

namespace AlishPustakGhar.Services.Interfaces;

public interface IBookService
{
    Task<string> AddBook(BookAddDto bookAddDto);
    
    void DeleteBook(Guid id);
    
    BookResponseDto GetPaginatedBooks(int page, int pageSize);
    
    
    
    Task<BookWithDetailsDto> GetBookById(Guid id);
    //void UpdateBook(Guid id, )
    
    Task<bool> ApplyBulkDiscount(BulkDiscountDto bulkDiscountDto);
    
    Task<BookResponseDto> SearchBooksByTitle(string title, int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBooksByGenre(Guid genreId, int page = 1, int pageSize = 12);
    Task<BookResponseDto> GetBooksByGenreType(string genreType, int page = 1, int pageSize = 1);
}