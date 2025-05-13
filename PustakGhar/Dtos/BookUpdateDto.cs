namespace AlishPustakGhar.Dtos;

// Add to your Dtos folder

public class BookUpdateDto
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Description { get; set; }
    public int? Price { get; set; }
    public int? Stock { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string Publisher { get; set; }
    public string ReleaseStatus { get; set; }
    public bool? IsOnSale { get; set; }
    public bool? IsBestSeller { get; set; }
    public DateTime? DiscountStartDate { get; set; }
    public DateTime? DiscountEndDate { get; set; }
    public int? DiscountPercentage { get; set; }
    public IFormFile FrontImage { get; set; }
    public IFormFile BackImage { get; set; }
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
}