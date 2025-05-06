using AlishPustakGhar.Dtos;
using AlishPustakGhar.Services.Interfaces;
using AlishPustakGhar.Utils;
using WebApplication1.Data;
using WebApplication1.Model;

namespace AlishPustakGhar.Services;

public class BookService:IBookService
{
    private ApplicationDbContext _context;
    
    private FileHelper _fileHelper;


    public BookService(ApplicationDbContext context, FileHelper fileHelper)
    {
        _context = context;
        _fileHelper = fileHelper;
    }

    public async Task<string> AddBook(BookAddDto bookAddDto)
    {
        if (bookAddDto==null)
        {
            return null;
        }

        if (bookAddDto.FrontImage==null || bookAddDto.BackImage==null)
        {
            throw new Exception("Image not found");
        }
       var frontFileName = await  _fileHelper.SaveFile(bookAddDto.FrontImage, "Books");
       
       var backFileName = await  _fileHelper.SaveFile(bookAddDto.BackImage, "Books");
       
       
        _fileHelper.SaveFile(bookAddDto.BackImage, "Books");
        
        var book = new Book
        {
            Description = bookAddDto.Description,
            // inititlize other fields
            Title = bookAddDto.Title,
            ISBN = bookAddDto.ISBN,
            Price = bookAddDto.Price,
            Stock = bookAddDto.Stock,
            PublishedDate = bookAddDto.PublishedDate,
            Publisher = bookAddDto.Publisher,
            ReleaseStatus = bookAddDto.ReleaseStatus,
            IsOnSale = bookAddDto.IsOnSale,
            FrontImage = frontFileName,
            BackImage = backFileName,
            
        };
        
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book.Id.ToString();

    }
    public void DeleteBook(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Book> GetAllBooks()
    {
       return _context.Books.ToList();
    }

    public Book? GetBookById(Guid id)
    {
        return _context.Books.FirstOrDefault(x => x.Id == id);
    }
}