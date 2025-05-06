using AlishPustakGhar.Dtos;
using WebApplication1.Model;

namespace AlishPustakGhar.Services.Interfaces;

public interface IBookService
{
    Task<string> AddBook(BookAddDto bookAddDto);
    
    void DeleteBook(Guid id);
    
    List<Book> GetAllBooks();
    
    
    
    Book GetBookById(Guid id);
    //void UpdateBook(Guid id, )
}