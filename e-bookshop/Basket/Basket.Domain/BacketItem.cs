namespace Basket.Domain
{
    public class BasketItem
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
