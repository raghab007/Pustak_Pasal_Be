using AlishPustakGhar.Dtos;
using WebApplication1.Model;

namespace AlishPustakGhar.Services.Interfaces;

public interface IBookService
{
    Task<string> AddBook(BookAddDto bookAddDto);
    
    void DeleteBook(Guid id);
    
    BookResponseDto GetPaginatedBooks(int page, int pageSize);
    
    
    
    Book GetBookById(Guid id);
    //void UpdateBook(Guid id, )
}