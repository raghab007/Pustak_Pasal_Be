using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model;



[Table("AuthorBook")]
public class AuthorBook
{
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [ForeignKey(nameof(Author))]
    public Guid AuthorId { get; set; }
    
    public virtual Author Author { get; set; }
    
    [ForeignKey(nameof(Book))]
    public Guid BookId { get; set; }
    
    public virtual Book Book { get; set; }

}