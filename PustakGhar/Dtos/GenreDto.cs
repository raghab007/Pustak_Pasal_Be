
using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

public class GenreDto
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "Genre type is required")]
    public string GenreType { get; set; }
}