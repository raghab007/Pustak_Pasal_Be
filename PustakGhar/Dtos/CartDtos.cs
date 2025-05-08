namespace AlishPustakGhar.Dtos;

public class CartDtos
{
    
}

public class CartResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<CartItemResponse> Items { get; set; } = new();
    public CartSummary Summary { get; set; } = new();
}

public class CartItemResponse
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int DiscountedPrice { get; set; }
    public bool IsDiscountActive { get; set; }
    public int DiscountPercentage { get; set; }
    public int AvailableStock { get; set; }
}

public class CartSummary
{
    public int TotalItems { get; set; }
    public int Subtotal { get; set; }
    public int TotalDiscount { get; set; }
    public int GrandTotal { get; set; }
}