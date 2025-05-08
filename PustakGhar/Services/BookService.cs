using System.ComponentModel.DataAnnotations;
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Services.Interfaces;
using AlishPustakGhar.Utils;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Adds a new book to the database.
    /// </summary>
    /// <param name="bookAddDto">The data transfer object containing book details</param>
    /// <returns>The ID of the newly created book as a string</returns>
    /// <exception cref="Exception">Thrown when required images are missing</exception>
    
    

    public async Task<string> AddBookAgain(BookAddDto bookAddDto)
    {
        // Validate input
        if (bookAddDto == null)
        {
            throw new ArgumentNullException(nameof(bookAddDto));
        }

        // Validate required images
        if (bookAddDto.FrontImage == null || bookAddDto.BackImage == null)
        {
            throw new ValidationException("Both front and back images are required");
        }

        string frontFileName = null;
        string backFileName = null;

        try
        {
            // Save images
            frontFileName = await _fileHelper.SaveFile(bookAddDto.FrontImage, "Books");
            backFileName = await _fileHelper.SaveFile(bookAddDto.BackImage, "Books");

            // Create new book
            var book = new Book
            {
                Title = bookAddDto.Title,
                ISBN = bookAddDto.ISBN,
                Description = bookAddDto.Description,
                Price = bookAddDto.Price,
                Stock = bookAddDto.Stock,
                PublishedDate = bookAddDto.PublishedDate,
                Publisher = bookAddDto.Publisher,
                ReleaseStatus = bookAddDto.ReleaseStatus,
                IsOnSale = bookAddDto.IsOnSale,
                IsBestSeller = bookAddDto.IsBestSeller,
                FrontImage = frontFileName,
                BackImage = backFileName,
                DiscoundStartDate = bookAddDto.DiscountStartDate,
                DiscoundEndDate = bookAddDto.DiscountEndDate,
                AuthorBooks = new List<AuthorBook>()
            };

            // Process authors
            if (bookAddDto.Authors != null && bookAddDto.Authors.Any())
            {
                foreach (var authorDto in bookAddDto.Authors)
                {
                    if (string.IsNullOrWhiteSpace(authorDto.Name))
                        continue;

                    var normalizedName = authorDto.Name.Trim().ToLowerInvariant();
                    
                    // Check for existing author by name
                    var existingAuthor = await _context.Authors
                        .FirstOrDefaultAsync(a => a.Name.ToLower() == normalizedName);

                    if (existingAuthor != null)
                    {
                        // Update existing author if new information is provided
                        if (!string.IsNullOrWhiteSpace(authorDto.Bio))
                        {
                            existingAuthor.Bio = authorDto.Bio;
                        }
                        

                        // Use existing author
                        book.AuthorBooks.Add(new AuthorBook
                        {
                            AuthorId = existingAuthor.Id,
                            BookId = book.Id
                        });
                    }
                    else
                    {
                        // Create new author
                        var newAuthor = new Author
                        {
                            Name = authorDto.Name.Trim(),
                            Bio = authorDto.Bio ?? string.Empty,
                            RegisteredDate = DateTime.UtcNow
                        };

                       

                        // Add the author relationship
                        book.AuthorBooks.Add(new AuthorBook
                        {
                            Author = newAuthor,
                            BookId = book.Id
                        });
                    }
                }
            }

            // Add and save
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id.ToString();
        }
        catch (Exception ex)
        {
            // Clean up saved files if something went wrong
            if (frontFileName != null) _fileHelper.DeleteFile(frontFileName, "Books");
            if (backFileName != null) _fileHelper.DeleteFile(backFileName, "Books");
            
            throw new Exception("Error adding book", ex);
        }
    }    
    
    
    
    public async Task<string> AddBookAgain2(BookAddDto bookAddDto)
{
    // Validate input
    if (bookAddDto == null)
    {
        throw new ArgumentNullException(nameof(bookAddDto));
    }

    // Validate required images
    if (bookAddDto.FrontImage == null || bookAddDto.BackImage == null)
    {
        throw new ValidationException("Both front and back images are required");
    }

    string frontFileName = null;
    string backFileName = null;

    try
    {
        // Save images
        frontFileName = await _fileHelper.SaveFile(bookAddDto.FrontImage, "Books");
        backFileName = await _fileHelper.SaveFile(bookAddDto.BackImage, "Books");

        // Create new book
        var book = new Book
        {
            Title = bookAddDto.Title,
            ISBN = bookAddDto.ISBN,
            Description = bookAddDto.Description,
            Price = bookAddDto.Price,
            Stock = bookAddDto.Stock,
            PublishedDate = bookAddDto.PublishedDate,
            Publisher = bookAddDto.Publisher,
            ReleaseStatus = bookAddDto.ReleaseStatus,
            IsOnSale = bookAddDto.IsOnSale,
            IsBestSeller = bookAddDto.IsBestSeller,
            FrontImage = frontFileName,
            BackImage = backFileName,
            DiscoundStartDate = bookAddDto.DiscountStartDate,
            DiscoundEndDate = bookAddDto.DiscountEndDate,
            AuthorBooks = new List<AuthorBook>(),
            BookGenres = new List<BookGenre>()
        };

        // Process authors
        if (bookAddDto.Authors != null && bookAddDto.Authors.Any())
        {
            foreach (var authorDto in bookAddDto.Authors)
            {
                if (string.IsNullOrWhiteSpace(authorDto.Name))
                    continue;

                var normalizedName = authorDto.Name.Trim().ToLowerInvariant();
                
                // Check for existing author by name
                var existingAuthor = await _context.Authors
                    .FirstOrDefaultAsync(a => a.Name.ToLower() == normalizedName);

                if (existingAuthor != null)
                {
                    // Update existing author if new information is provided
                    if (!string.IsNullOrWhiteSpace(authorDto.Bio))
                    {
                        existingAuthor.Bio = authorDto.Bio;
                    }

                    // Use existing author
                    book.AuthorBooks.Add(new AuthorBook
                    {
                        AuthorId = existingAuthor.Id,
                        BookId = book.Id
                    });
                }
                else
                {
                    // Create new author
                    var newAuthor = new Author
                    {
                        Name = authorDto.Name.Trim(),
                        Bio = authorDto.Bio ?? string.Empty,
                        RegisteredDate = DateTime.UtcNow
                    };

                    // Add the author relationship
                    book.AuthorBooks.Add(new AuthorBook
                    {
                        Author = newAuthor,
                        BookId = book.Id
                    });
                }
            }
        }

        // Process genres
        if (bookAddDto.Genres != null && bookAddDto.Genres.Any())
        {
            foreach (var genreDto in bookAddDto.Genres)
            {
                // Check for existing genre by type
                var existingGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.GenreType == genreDto.GenreType);

                if (existingGenre != null)
                {
                    // Use existing genre
                    book.BookGenres.Add(new BookGenre
                    {
                        GenreId = existingGenre.Id,
                        BookId = book.Id
                    });
                }
                else
                {
                    // Create new genre
                    var newGenre = new Genre
                    {
                        GenreType = genreDto.GenreType
                    };

                    // Add the genre relationship
                    book.BookGenres.Add(new BookGenre
                    {
                        Genre = newGenre,
                        BookId = book.Id
                    });
                }
            }
        }

        // Add and save
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book.Id.ToString();
    }
    catch (Exception ex)
    {
        // Clean up saved files if something went wrong
        if (frontFileName != null) _fileHelper.DeleteFile(frontFileName, "Books");
        if (backFileName != null) _fileHelper.DeleteFile(backFileName, "Books");
        
        throw new Exception("Error adding book", ex);
    }
}
    
    
    public async Task<string> AddBook(BookAddDto bookAddDto)
{
    // Validate input
    if (bookAddDto == null)
    {
        throw new ArgumentNullException(nameof(bookAddDto));
    }

    // Validate required images
    if (bookAddDto.FrontImage == null || bookAddDto.BackImage == null)
    {
        throw new ValidationException("Both front and back images are required");
    }

    string frontFileName = null;
    string backFileName = null;

    try
    {
        // Save images
        frontFileName = await _fileHelper.SaveFile(bookAddDto.FrontImage, "Books");
        backFileName = await _fileHelper.SaveFile(bookAddDto.BackImage, "Books");

        // Create new book
        var book = new Book
        {
            Title = bookAddDto.Title,
            ISBN = bookAddDto.ISBN,
            Description = bookAddDto.Description,
            Price = bookAddDto.Price,
            Stock = bookAddDto.Stock,
            PublishedDate = bookAddDto.PublishedDate,
            Publisher = bookAddDto.Publisher,
            ReleaseStatus = bookAddDto.ReleaseStatus,
            IsOnSale = bookAddDto.IsOnSale,
            IsBestSeller = bookAddDto.IsBestSeller,
            FrontImage = frontFileName,
            BackImage = backFileName,
            DiscoundStartDate = bookAddDto.DiscountStartDate,
            DiscoundEndDate = bookAddDto.DiscountEndDate,
            AuthorBooks = new List<AuthorBook>(),
            BookGenres = new List<BookGenre>()
        };

        // Process authors
        if (bookAddDto.Authors != null && bookAddDto.Authors.Any())
        {
            foreach (var authorDto in bookAddDto.Authors)
            {
                if (string.IsNullOrWhiteSpace(authorDto.Name))
                    continue;

                var normalizedName = authorDto.Name.Trim().ToLowerInvariant();
                
                var existingAuthor = await _context.Authors
                    .FirstOrDefaultAsync(a => a.Name.ToLower() == normalizedName);

                if (existingAuthor != null)
                {
                    if (!string.IsNullOrWhiteSpace(authorDto.Bio))
                    {
                        existingAuthor.Bio = authorDto.Bio;
                    }
                    
                    book.AuthorBooks.Add(new AuthorBook
                    {
                        AuthorId = existingAuthor.Id,
                        BookId = book.Id
                    });
                }
                else
                {
                    var newAuthor = new Author
                    {
                        Name = authorDto.Name.Trim(),
                        Bio = authorDto.Bio ?? string.Empty,
                        RegisteredDate = DateTime.UtcNow
                    };

                    book.AuthorBooks.Add(new AuthorBook
                    {
                        Author = newAuthor,
                        BookId = book.Id
                    });
                }
            }
        }

        // Process genres (using string comparison)
        if (bookAddDto.Genres != null && bookAddDto.Genres.Any())
        {
            foreach (var genreDto in bookAddDto.Genres)
            {
                if (string.IsNullOrWhiteSpace(genreDto.GenreType))
                    continue;

                var normalizedGenreType = genreDto.GenreType.Trim().ToLowerInvariant();
                
                var existingGenre = await _context.Genres
                    .FirstOrDefaultAsync(g => g.GenreType.ToLower() == normalizedGenreType);

                if (existingGenre != null)
                {
                    book.BookGenres.Add(new BookGenre
                    {
                        GenreId = existingGenre.Id,
                        BookId = book.Id
                    });
                }
                else
                {
                    var newGenre = new Genre
                    {
                        GenreType = genreDto.GenreType.Trim()
                    };

                    book.BookGenres.Add(new BookGenre
                    {
                        Genre = newGenre,
                        BookId = book.Id
                    });
                }
            }
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return book.Id.ToString();
    }
    catch (Exception ex)
    {
        if (frontFileName != null) _fileHelper.DeleteFile(frontFileName, "Books");
        if (backFileName != null) _fileHelper.DeleteFile(backFileName, "Books");
        
        throw new Exception("Error adding book", ex);
    }
}
    
    
    public void DeleteBook(Guid id)
    {
        throw new NotImplementedException();
    }

    public   BookResponseDto GetPaginatedBooks(int page=1, int pageSize=12)
    {

        var query = _context.Books.AsQueryable();
        var totalCount = query.Count();
        var books = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var list = _context.Books.ToList();
        BookResponseDto bookResponseDto = new BookResponseDto
        {
            Books = books.Select(book => new BookViewDto(book)).ToList(),
            TotalCount = totalCount
        };
        
        return bookResponseDto;
    }

    public Book? GetBookById(Guid id)
    {
        return _context.Books.FirstOrDefault(x => x.Id == id);
    }
}