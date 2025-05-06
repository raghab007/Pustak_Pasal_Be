using Microsoft.AspNetCore.Mvc;

namespace AlishPustakGhar.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly string _baseDirectory = "/Users/raghabpokhrel/Desktop/PustakGhar";

        [HttpGet("{folder}/{imageName}")]
        public IActionResult GetImage(string folder, string imageName)
        {
            // Only allow "Books" or "Users" to avoid abuse
            var allowedFolders = new[] { "Books", "Users" };
            if (!allowedFolders.Contains(folder))
                return BadRequest("Invalid folder");

            var fullPath = Path.Combine(_baseDirectory, folder, imageName);
            if (!System.IO.File.Exists(fullPath))
                return NotFound("Image not found");

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var contentType = GetContentType(imageName);
            return File(stream, contentType);
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }
    }

}
