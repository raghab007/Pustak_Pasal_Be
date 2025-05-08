using System.ComponentModel.DataAnnotations;

namespace AlishPustakGhar.Dtos;

public class BookWithDetailsDto
{
    
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; }

    public string ISBN { get; set; }
    public DateTime? PublishedDate { get; set; }
        
    public String Publisher { get; set; }
    public string Description { get; set; }
        
    public int Price { get; set; }
        
    public int Stock { get; set; }
        
    public bool IsAvailable { get; set; } = true;
        
    public string FrontImage { get; set; }
        
    public string BackImage { get; set; }
        
    public int TotalSold { get; set; }
        
        
    public int TotalReviews { get; set; }
        
    public string ReleaseStatus { get; set; }

    public bool IsOnSale { get; set; } = false;
        
    public bool IsBestSeller { get; set; } = false;
        
    public  DateTime? DiscoundStartDate { get; set; }
        
    public  DateTime? DiscoundEndDate { get; set; }

    
    public List<AuthorDto> Authors { get; set; } = new();
    public List<GenreDto> Genres { get; set; } = new();
}

