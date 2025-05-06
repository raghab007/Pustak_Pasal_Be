using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Enums;

namespace WebApplication1.Model
{


    [Table("Genre")]
    public class Genre
    {

        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public GenreType GenreType { get; set; }
        
        
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    }
}