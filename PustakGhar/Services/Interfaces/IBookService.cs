using AlishPustakGhar.Dtos;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;

namespace AlishPustakGhar.Services.Interfaces;

public interface IBookService
{
    Task<string> AddBook(BookAddDto bookAddDto);
    
    void DeleteBook(Guid id);
    
    BookResponseDto GetPaginatedBooks(int page, int pageSize);
    
    
    
    Task<BookWithDetailsDto> GetBookById(Guid id);
    //void UpdateBook(Guid id, )
}