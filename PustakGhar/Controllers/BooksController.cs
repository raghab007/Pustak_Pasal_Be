
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlishPustakGhar.Controllers;


[ApiController]
[Route("api/[controller]")]
public class BooksController: ControllerBase
{
    
    IBookService _bookService;
    
    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBook([FromForm] BookAddDto bookAddDto)
    {
        string idBook = await _bookService.AddBook(bookAddDto);
        return Ok(idBook);
    }
    
    [HttpGet]
    public  IActionResult GetAllBooks()
    {
        var books =  _bookService.GetAllBooks();
        return Ok(books);
    }
    
    [HttpGet("id:Guid")]
    public  IActionResult GetBook(Guid id)
    {
        var book =  _bookService.GetBookById(id);
        return Ok(book);
    }
}