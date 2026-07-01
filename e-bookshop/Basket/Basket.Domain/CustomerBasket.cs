namespace Basket.Domain;

public class CustomerBasket
{
    public Guid UserId { get; set; }
    public List<BasketItem> Items { get; set; } = [];

    public decimal TotalPrice => Items.Sum(i => i.UnitPrice * i.Quantity);
}