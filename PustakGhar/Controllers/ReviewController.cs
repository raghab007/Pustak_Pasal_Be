
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Services.Interfaces;

namespace AlishPustakGhar.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto reviewDto)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            try
            {
                var review = await _reviewService.AddReview(userId, reviewDto);
                return Ok(review);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("book/{bookId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookReviews(Guid bookId)
        {
            var reviews = await _reviewService.GetReviewsForBook(bookId);
            return Ok(reviews);
        }

        [HttpGet("has-reviewed/{bookId}")]
        public async Task<IActionResult> HasUserReviewedBook(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var hasReviewed = await _reviewService.HasUserReviewedBook(userId, bookId);
            return Ok(hasReviewed);
        }
    }
}