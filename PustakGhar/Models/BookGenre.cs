using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model;


[Table("BookGenre")]
public class BookGenre
{
    
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    
    public virtual Book Book { get; set; }
    
    [ForeignKey(nameof(Genre))]
    public Guid GenreId { get; set; }
    
    public Genre Genre { get; set; }
    
}