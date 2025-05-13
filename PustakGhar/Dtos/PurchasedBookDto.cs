
using AlishPustakGhar.Dtos;

    public class PurchasedBookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public string FrontImage { get; set; }
        public ReviewDto UserReview { get; set; } // Null if not reviewed
    }
