namespace AlishPustakGhar.Models.DTOs
{
    public class ClaimCodeDetailsDto
    {
        public OrderInfoDto Order { get; set; }
        public UserInfoDto User { get; set; }
        public List<BookInfoDto> Books { get; set; } = new List<BookInfoDto>();
    }

    public class OrderInfoDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountApplied { get; set; }
        public string ClaimCode { get; set; }
    }

    public class UserInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class BookInfoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }
}