
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
    //[Consumes("multipart/form-data")]
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
    
    [HttpGet("{id}")]
    public async Task<ActionResult<BookWithDetailsDto>> GetBook(Guid id)
    {
        var bookDto = await _bookService.GetBookById(id);
        if (bookDto == null)
        {
            return NotFound();
        }
        return Ok(bookDto);
    }
    
    [HttpPost("bulk-discount")]
    public async Task<IActionResult> ApplyBulkDiscount([FromBody] BulkDiscountDto dto)
    {
        try
        {
            var result = await _bookService.ApplyBulkDiscount(dto);
            return Ok(new { Success = result });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchBooks([FromQuery] string title, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        try
        {
            var result = await _bookService.SearchBooksByTitle(title, page, pageSize);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("by-genre/{genreId}")]
    public async Task<IActionResult> GetBooksByGenre(Guid genreId, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        try
        {
            var result = await _bookService.GetBooksByGenre(genreId, page, pageSize);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("by-genre-type")]
    public async Task<IActionResult> GetBooksByGenreType([FromQuery] string genreType, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        try
        {
            var result = await _bookService.GetBooksByGenreType(genreType, page, pageSize);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}