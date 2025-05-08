
using System.ComponentModel.DataAnnotations;
using WebApplication1.Enums;

namespace AlishPustakGhar.Dtos;

public class GenreDto
{
    public Guid? Id { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "Genre type is required")]
    public string GenreType { get; set; }
}