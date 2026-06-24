namespace Ordering.Application.DTOs
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; } = 1;
    }
}
