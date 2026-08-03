namespace Basket.Application.DTOs;

public class BasketItemDto
{
    public Guid BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } = 0;
    public int Quantity { get; set; } = 1;
}