using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Model
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string? ImageURL { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }
        
        [Required]
        public String Email { get; set; }
        
        [Range(1,100,ErrorMessage ="Please enter a number between 1 and 100")]
        public int Age { get; set; }
    }
}