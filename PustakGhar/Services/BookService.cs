using System.ComponentModel.DataAnnotations;
using AlishPustakGhar.Data;
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Model;
using AlishPustakGhar.Services.Interfaces;
using AlishPustakGhar.Utils;
using Microsoft.EntityFrameworkCore;

namespace AlishPustakGhar.Services
{

    public class BookService : IBookService
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
                var book = new Book()
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

       

        public BookResponseDto GetPaginatedBooks(int page = 1, int pageSize = 12)
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

        public async Task<BookWithDetailsDto>GetBookById(Guid id)
        {

            var bookDto = await _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookWithDetailsDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    PublishedDate = b.PublishedDate,
                    Publisher = b.Publisher,
                    Description = b.Description,
                    Price = b.Price,
                    Stock = b.Stock,
                    IsAvailable = b.IsAvailable,
                    FrontImage = b.FrontImage,
                    BackImage = b.BackImage,
                    TotalSold = b.TotalSold,
                    TotalReviews = b.TotalReviews,
                    ReleaseStatus = b.ReleaseStatus,
                    IsOnSale = b.IsOnSale,
                    IsBestSeller = b.IsBestSeller,
                    DiscoundStartDate = b.DiscoundStartDate,
                    DiscoundEndDate = b.DiscoundEndDate,
                    Authors = b.AuthorBooks.Select(ab => new AuthorDto
                    {
                        Id = ab.Author.Id,
                        Name = ab.Author.Name,
                        Bio = ab.Author.Bio, // Assuming Author has these properties
                    }).ToList(),
                    Genres = b.BookGenres.Select(bg => new GenreDto
                    {
                        Id = bg.Genre.Id,
                        GenreType = bg.Genre.GenreType,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            

            return (bookDto);

        }
        public async Task<bool> ApplyBulkDiscount(BulkDiscountDto bulkDiscountDto)
        {
            if (bulkDiscountDto == null)
            {
                throw new ArgumentNullException(nameof(bulkDiscountDto));
            }

            if (bulkDiscountDto.StartDate >= bulkDiscountDto.EndDate)
            {
                throw new ValidationException("End date must be after start date");
            }

            if (!bulkDiscountDto.BookIds.Any())
            {
                throw new ValidationException("At least one book must be selected");
            }

            var books = await _context.Books
                .Where(b => bulkDiscountDto.BookIds.Contains(b.Id))
                .ToListAsync();

            if (books.Count != bulkDiscountDto.BookIds.Count)
            {
                // Some books weren't found - you might want to handle this differently
                throw new KeyNotFoundException("One or more books could not be found");
            }

            foreach (var book in books)
            {
                book.DiscountPercentage = bulkDiscountDto.DiscountPercentage;
                book.DiscoundStartDate = bulkDiscountDto.StartDate;
                book.DiscoundEndDate = bulkDiscountDto.EndDate;
                book.IsOnSale = bulkDiscountDto.SetOnSale;
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the error here
                throw new Exception("Failed to apply bulk discount", ex);
            }
        }
        
        
        // Add these methods to your BookService class

public async Task<BookResponseDto> SearchBooksByTitle(string title, int page = 1, int pageSize = 12)
{
    if (string.IsNullOrWhiteSpace(title))
    {
        throw new ArgumentException("Title cannot be empty", nameof(title));
    }

    var query = _context.Books
        .Where(b => b.Title.Contains(title))
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

public async Task<BookResponseDto> GetBooksByGenre(Guid genreId, int page = 1, int pageSize = 12)
{
    var genreExists = await _context.Genres.AnyAsync(g => g.Id == genreId);
    if (!genreExists)
    {
        throw new KeyNotFoundException("Genre not found");
    }

    var query = _context.Books
        .Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId))
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

// Also add this method if you want to get books by genre name (type)
public async Task<BookResponseDto> GetBooksByGenreType(string genreType, int page = 1, int pageSize = 12)
{
    if (string.IsNullOrWhiteSpace(genreType))
    {
        throw new ArgumentException("Genre type cannot be empty", nameof(genreType));
    }

    var normalizedGenreType = genreType.Trim().ToLowerInvariant();

    var query = _context.Books
        .Where(b => b.BookGenres.Any(bg => 
            bg.Genre.GenreType.ToLower() == normalizedGenreType))
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
    }


// Add to BookService.cs

public async Task<bool> DeleteBook(Guid id)
{
    var book = await _context.Books
        .Include(b => b.AuthorBooks)
        .Include(b => b.BookGenres)
        .FirstOrDefaultAsync(b => b.Id == id);

    if (book == null)
    {
        return false;
    }

    try
    {
        // Delete associated images
        if (!string.IsNullOrEmpty(book.FrontImage))
        {
            _fileHelper.DeleteFile(book.FrontImage, "Books");
        }
        
        if (!string.IsNullOrEmpty(book.BackImage))
        {
            _fileHelper.DeleteFile(book.BackImage, "Books");
        }

        // Remove the book
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
    catch (Exception ex)
    {
        // Log error
        throw new Exception("Error deleting book", ex);
    }
}

public async Task<bool> UpdateBook(Guid id, BookUpdateDto bookUpdateDto)
{
    var book = await _context.Books
        .Include(b => b.AuthorBooks)
        .Include(b => b.BookGenres)
        .FirstOrDefaultAsync(b => b.Id == id);

    if (book == null)
    {
        return false;
    }

    try
    {
        // Update basic properties
        book.Title = bookUpdateDto.Title ?? book.Title;
        book.ISBN = bookUpdateDto.ISBN ?? book.ISBN;
        book.Description = bookUpdateDto.Description ?? book.Description;
        book.Price = bookUpdateDto.Price ?? book.Price;
        book.Stock = bookUpdateDto.Stock ?? book.Stock;
        book.PublishedDate = bookUpdateDto.PublishedDate ?? book.PublishedDate;
        book.Publisher = bookUpdateDto.Publisher ?? book.Publisher;
        book.ReleaseStatus = bookUpdateDto.ReleaseStatus ?? book.ReleaseStatus;
        book.IsOnSale = bookUpdateDto.IsOnSale ?? book.IsOnSale;
        book.IsBestSeller = bookUpdateDto.IsBestSeller ?? book.IsBestSeller;
        book.DiscoundStartDate = bookUpdateDto.DiscountStartDate ?? book.DiscoundStartDate;
        book.DiscoundEndDate = bookUpdateDto.DiscountEndDate ?? book.DiscoundEndDate;
        book.DiscountPercentage = bookUpdateDto.DiscountPercentage ?? book.DiscountPercentage;

        // Handle image updates
        if (bookUpdateDto.FrontImage != null)
        {
            var newFrontImage = await _fileHelper.SaveFile(bookUpdateDto.FrontImage, "Books");
            _fileHelper.DeleteFile(book.FrontImage, "Books");
            book.FrontImage = newFrontImage;
        }

        if (bookUpdateDto.BackImage != null)
        {
            var newBackImage = await _fileHelper.SaveFile(bookUpdateDto.BackImage, "Books");
            _fileHelper.DeleteFile(book.BackImage, "Books");
            book.BackImage = newBackImage;
        }

        // Update authors if provided
        if (bookUpdateDto.Authors != null)
        {
            // Remove existing authors not in the update list
            var authorsToRemove = book.AuthorBooks
                .Where(ab => !bookUpdateDto.Authors.Any(a => a.Id == ab.AuthorId))
                .ToList();

            foreach (var author in authorsToRemove)
            {
                book.AuthorBooks.Remove(author);
            }

            // Add or update authors
            foreach (var authorDto in bookUpdateDto.Authors)
            {
                var existingAuthorBook = book.AuthorBooks.FirstOrDefault(ab => ab.AuthorId == authorDto.Id);
                
                if (existingAuthorBook == null)
                {
                    var author = await _context.Authors.FindAsync(authorDto.Id);
                    if (author != null)
                    {
                        book.AuthorBooks.Add(new AuthorBook
                        {
                            AuthorId = author.Id,
                            BookId = book.Id
                        });
                    }
                }
            }
        }

        // Update genres if provided
        if (bookUpdateDto.Genres != null)
        {
            // Remove existing genres not in the update list
            var genresToRemove = book.BookGenres
                .Where(bg => !bookUpdateDto.Genres.Any(g => g.Id == bg.GenreId))
                .ToList();

            foreach (var genre in genresToRemove)
            {
                book.BookGenres.Remove(genre);
            }

            // Add or update genres
            foreach (var genreDto in bookUpdateDto.Genres)
            {
                var existingBookGenre = book.BookGenres.FirstOrDefault(bg => bg.GenreId == genreDto.Id);
                
                if (existingBookGenre == null)
                {
                    var genre = await _context.Genres.FindAsync(genreDto.Id);
                    if (genre != null)
                    {
                        book.BookGenres.Add(new BookGenre
                        {
                            GenreId = genre.Id,
                            BookId = book.Id
                        });
                    }
                }
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }
    catch (Exception ex)
    {
        // Log error
        throw new Exception("Error updating book", ex);
    }
}

public async Task<BookResponseDto> GetComingSoonBooks(int page = 1, int pageSize = 12)
{
    var query = _context.Books
        .Where(b => b.ReleaseStatus == "Coming Soon")
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

public async Task<BookResponseDto> GetBooksOnSale(int page = 1, int pageSize = 12)
{
    var now = DateTime.UtcNow;
    
    var query = _context.Books
        .Where(b => b.IsOnSale && 
                    b.DiscoundStartDate <= now && 
                    b.DiscoundEndDate >= now)
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

public async Task<BookResponseDto> GetBestSellers(int page = 1, int pageSize = 12)
{
    var query = _context.Books
        .Where(b => b.IsBestSeller)
        .OrderByDescending(b => b.TotalSold)
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

public async Task<BookResponseDto> GetNewReleases(int page = 1, int pageSize = 12)
{
    var threeMonthsAgo = DateTime.UtcNow.AddMonths(-3);
    
    var query = _context.Books
        .Where(b => b.PublishedDate >= threeMonthsAgo)
        .OrderByDescending(b => b.PublishedDate)
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}

public async Task<BookResponseDto> GetNewArrivals(int page = 1, int pageSize = 12)
{
    var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
    
    var query = _context.Books
        .Where(b => b.PublishedDate >= oneMonthAgo)
        .OrderByDescending(b => b.PublishedDate)
        .AsQueryable();

    var totalCount = await query.CountAsync();
    var books = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new BookResponseDto
    {
        Books = books.Select(book => new BookViewDto(book)).ToList(),
        TotalCount = totalCount
    };
}



public async Task<List<PurchasedBookDto>> GetPurchasedBooks(Guid userId)
{
    return await _context.Orders
        .Where(o => o.UserId == userId && o.OrderStatus == "Delivered")
        .Include(o => o.OrderItems)
        .ThenInclude(oi => oi.Book)
        .ThenInclude(b => b.Reviews)
        .SelectMany(o => o.OrderItems.Select(oi => oi.Book))
        .Distinct()
        .Select(b => new PurchasedBookDto
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            Price = b.Price,
            FrontImage = b.FrontImage,
            UserReview = b.Reviews
                .Where(r => r.UserId == userId)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .FirstOrDefault()
        })
        .ToListAsync();
}

    }
    
}
