using Bookshop.SharedKernel.Domain;
using Bookshop.SharedKernel.Domain.Common.Events;
using Ordering.Domain.Enums;

namespace Ordering.Domain
{
    public record OrderPlacedEvent(Guid Id, Money TotalCost) : IDomainEvent;
    public record OrderCancelledEvent(Guid Id) : IDomainEvent;
    public class Order
    {

        private readonly List<IDomainEvent> _domainEvents = [];
        public Guid Id { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = [];
        public DateTime PlacedDate { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public Address Address { get; private set; }
        public Money TotalCost { get; private set; }
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        private Order() { }

        public static Order Place(Guid id, ICollection<OrderItem> items, Address address, Money totalCost)
        {
            var order = new Order
            {
                Id = id,
                OrderItems = items,
                Address = address,
                TotalCost = totalCost,
                OrderStatus = OrderStatus.Pending,
                PlacedDate = DateTime.UtcNow
            };

            order.AddDomainEvent(new OrderPlacedEvent(order.Id, order.TotalCost));
            return order;
        }

        private static readonly Dictionary<OrderStatus, IEnumerable<OrderStatus>> _validTransitions = new()
        {
            [OrderStatus.Pending] = [OrderStatus.Confirmed, OrderStatus.Cancelled],
            [OrderStatus.Confirmed] = [OrderStatus.Shipped, OrderStatus.Cancelled],
            [OrderStatus.Shipped] = [OrderStatus.Delivered, OrderStatus.Cancelled],
            [OrderStatus.Delivered] = [],
            [OrderStatus.Cancelled] = [],
        };

        public void TransitionStatus(OrderStatus newStatus)
        {
            if (!_validTransitions[OrderStatus].Contains(newStatus))
                throw new InvalidOperationException(
                    $"Cannot transition from {OrderStatus} to {newStatus}.");

            OrderStatus = newStatus;
            if (newStatus == OrderStatus.Cancelled)
                AddDomainEvent(new OrderCancelledEvent(Id));
        }

        public void UpdateItemQuantity(Guid itemId, int newQuantity)
        {
            if (OrderStatus != OrderStatus.Pending)
                throw new InvalidOperationException("Cannot modify items on a non-pending order.");

            var item = OrderItems.FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException($"OrderItem {itemId} not found.");

            item.UpdateQuantity(newQuantity);
            TotalCost = RecalculateTotal();
        }
        private Money RecalculateTotal() => new(OrderItems.Sum(i => i.Price.Amount * i.Quantity), TotalCost.Currency);
        public void ClearDomainEvents() => _domainEvents.Clear();
        private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    }

    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string Country { get; }
        public string Postcode { get; }
        private Address() { }
        public override string ToString()
        {
            return $"{Street}, {City}, {Country} {Postcode}";
        }
        public Address(string street, string city, string country, string postcode)
        {
            Street = street;
            City = city;
            Country = country;
            Postcode = postcode;
        }
    }
}
