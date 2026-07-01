namespace Basket.Application.DTOs;

public class ShowCustomerBasketDto
{
    public Guid UserId { get; set; }
    public List<BasketItemDto> Items { get; set; } = [];
    public decimal TotalPrice { get; set; }
}