// FavouritesController.cs
using AlishPustakGhar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlishPustakGhar.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FavouritesController : ControllerBase
    {
        private readonly IFavouritesService _favouritesService;

        public FavouritesController(IFavouritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToFavourites([FromBody] AddToFavouritesRequest request)
        {
            // Get current user ID
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userId == Guid.Empty || request.BookId == Guid.Empty)
            {
                return BadRequest("Invalid user or book ID");
            }

            var result = await _favouritesService.AddToFavouritesAsync(userId, request.BookId);

            if (!result)
            {
                return BadRequest("Unable to add to favourites. The book may already be in your favourites.");
            }

            return Ok(new { Success = true, Message = "Book added to favourites successfully" });
        }

        [HttpDelete("remove/{bookId}")]
        public async Task<IActionResult> RemoveFromFavourites(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userId == Guid.Empty || bookId == Guid.Empty)
            {
                return BadRequest("Invalid user or book ID");
            }

            var result = await _favouritesService.RemoveFromFavouritesAsync(userId, bookId);

            if (!result)
            {
                return BadRequest("Book not found in favourites");
            }

            return Ok(new { Success = true, Message = "Book removed from favourites successfully" });
        }

        [HttpGet("check/{bookId}")]
        public async Task<IActionResult> IsBookInFavourites(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userId == Guid.Empty || bookId == Guid.Empty)
            {
                return BadRequest("Invalid user or book ID");
            }

            var result = await _favouritesService.IsBookInFavouritesAsync(userId, bookId);

            return Ok(new { IsInFavourites = result });
        }
    }

    public class AddToFavouritesRequest
    {
        public Guid BookId { get; set; }
    }
}