namespace AlishPustakGhar.Dtos;

public class OrderWithUserResponse
{
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public UserInfo User { get; set; }
        
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}