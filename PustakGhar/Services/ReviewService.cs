

using AlishPustakGhar.Data;
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Models;
using AlishPustakGhar.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlishPustakGhar.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReviewDto> AddReview(Guid userId, CreateReviewDto reviewDto)
        {
            // Check if user has already reviewed this book
            if (await HasUserReviewedBook(userId, reviewDto.BookId))
            {
                throw new InvalidOperationException("You have already reviewed this book");
            }

            // Verify the user has purchased the book
            var hasPurchased = await _context.OrderItems
                .AnyAsync(oi => oi.BookId == reviewDto.BookId && 
                               oi.Order.UserId == userId && 
                               oi.Order.OrderStatus == "Delivered");

            if (!hasPurchased)
            {
                throw new InvalidOperationException("You must purchase the book before reviewing");
            }

            var review = new Review()
            {
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                BookId = reviewDto.BookId,
                UserId = userId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            // Update book's average rating
            await UpdateBookRating(reviewDto.BookId);

            var user = await _context.Users.FindAsync(userId);

            return new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                BookId = review.BookId,
                UserId = review.UserId,
                UserName = $"{user.FirstName} {user.LastName}"
            };
        }

        public async Task<bool> HasUserReviewedBook(Guid userId, Guid bookId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<List<ReviewDto>> GetReviewsForBook(Guid bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    BookId = r.BookId,
                    UserId = r.UserId,
                    UserName = $"{r.User.FirstName} {r.User.LastName}"
                })
                .ToListAsync();
        }

        private async Task UpdateBookRating(Guid bookId)
        {
            var book = await _context.Books
                .Include(b => b.Reviews)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book != null && book.Reviews.Any())
            {
                book.TotalReviews = book.Reviews.Count;
                // Optionally update average rating if you have that field
                // book.AverageRating = book.Reviews.Average(r => r.Rating);
                await _context.SaveChangesAsync();
            }
        }
    }
}