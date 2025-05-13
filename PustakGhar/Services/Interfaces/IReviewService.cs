using AlishPustakGhar.Dtos;

namespace AlishPustakGhar.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> AddReview(Guid userId, CreateReviewDto reviewDto);
        Task<bool> HasUserReviewedBook(Guid userId, Guid bookId);
        Task<List<ReviewDto>> GetReviewsForBook(Guid bookId);
    }
}