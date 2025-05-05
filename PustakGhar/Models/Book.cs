using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string BookName { get; set; }

        public string ISBN { get; set; }


        public string? Author { get; set; }

        public DateTime RegisteredDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }
    }
}