namespace AlishPustakGhar.Dtos;

public class UpdateCartItemRequest
{
    public Guid BookId { get; set; }
    public int Quantity { get; set; }
}