using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AlishPustakGhar.Dtos;

public class BookAddDto
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string ISBN { get; set; }
    
    public string Description { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    public DateTime? PublishedDate { get; set; }
    
    public string Publisher { get; set; }
    
    public string ReleaseStatus { get; set; }
    
    public bool IsOnSale { get; set; }
    
    public bool IsBestSeller { get; set; }
    
    public DateTime? DiscountStartDate { get; set; }
    
    public DateTime? DiscountEndDate { get; set; }
    
    [Required]
    public IFormFile FrontImage { get; set; }
    
    [Required]
    public IFormFile BackImage { get; set; }
    
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    
    public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
}