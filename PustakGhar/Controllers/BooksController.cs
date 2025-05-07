
using System.Text.Json;
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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddBook([FromForm] BookAddDto bookAddDto  )
    {

        string idBook = await _bookService.AddBook(bookAddDto);
        return Ok(idBook);
    }
    
    [HttpGet]
    public  IActionResult GetAllBooks(int page=1, int pageSize=12)
    {
        var books =  _bookService.GetPaginatedBooks(page, pageSize);
        return Ok(books);
    }
    
    [HttpGet("id:Guid")]
    public  IActionResult GetBook(Guid id)
    {
        var book =  _bookService.GetBookById(id);
        return Ok(book);
    }
}