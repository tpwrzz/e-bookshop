namespace Basket.Application.DTOs;

public class BasketDto
{
    public Guid UserId { get; set; }
    public List<BasketItemDto> Items { get; set; } = [];
}