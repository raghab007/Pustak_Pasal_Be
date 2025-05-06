using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    [Table("Book")]
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

        public string ISBN { get; set; }
        public DateTime PublishedDate { get; set; }
        
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
        
        public  DateTime DiscoundStartDate { get; set; }
        
        public  DateTime DiscoundEndDate { get; set; }
        
        public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
        
        public virtual ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        

    }
}