using System;
using System.Collections.Generic;

namespace AlishPustakGhar.Dtos;

public class CartDto
{
    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    public CartSummaryDto Summary { get; set; } = new CartSummaryDto();
}

public class CartItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Isbn { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountedPrice { get; set; }
    public bool IsDiscountActive { get; set; }
    public int Quantity { get; set; }
    public int AvailableStock { get; set; }
}

public class CartSummaryDto
{
    public decimal Subtotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal GrandTotal { get; set; }
} 