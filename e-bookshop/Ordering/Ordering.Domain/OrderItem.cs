using Bookshop.SharedKernel.Domain;

namespace Ordering.Domain
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }   
        public Money Price { get; private set; }
        public int Quantity { get; private set; }
        private OrderItem() { }
        public OrderItem(Guid id, string title, Money price, int quantity)
        {
            Id = id;
            Title = title;
            Price = price;
            Quantity = quantity;
        }
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity < 1)
                throw new ArgumentException("Quantity must be at least 1.");
            Quantity = newQuantity;
        }
    }
}