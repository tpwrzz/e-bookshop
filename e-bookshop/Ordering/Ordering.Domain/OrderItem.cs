using Bookshop.SharedKernel.Domain;

namespace Ordering.Domain
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }   
        public Money Price { get; private set; }
        public int Amount { get; private set; }
        private OrderItem() { }
        public OrderItem(Guid id, string title, Money price, int amount)
        {
            Id = id;
            Title = title;
            Price = price;
            Amount = amount;
        }
    }
}