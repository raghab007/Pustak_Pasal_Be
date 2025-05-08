using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Enums;

namespace AlishPustakGhar.Model
{


    [Table("Genre")]
    public class Genre
    {

        [Key] public Guid Id { get; set; } = Guid.NewGuid();

        public string GenreType { get; set; }
        
        
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

    }
}