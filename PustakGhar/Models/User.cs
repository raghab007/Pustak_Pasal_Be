using System.ComponentModel.DataAnnotations;
using AlishPustakGhar.Models;
using Microsoft.AspNetCore.Identity;

namespace AlishPustakGhar.Model
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string? ImageURL { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }
        
        
        [Range(1,100,ErrorMessage ="Please enter a number between 1 and 100")]
        public int Age { get; set; }
        
        public string Address { get; set; }
        
        public Guid? CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}