using Bookshop.SharedKernel.Domain;
using Ordering.Domain.Enums;
using Ordering.Domain.Events;

namespace Ordering.Domain
{
    public class Order
    {
        public Guid Id { get; private set; }
        public ICollection<OrderItem> OrderItems { get; private set; } = [];
        public OrderStatus OrderStatus { get; private set; }
        public Address Address { get; private set; }
        public Money TotalCost { get; private set; }
        private IReadOnlyList<IDomainEvent> DomainEvents { get; set; }
        private Order() { }

        public Order (Guid id, ICollection<OrderItem> orderItems, OrderStatus orderStatus, Address address, Money totalCost)
        {
            Id = id;
            OrderItems = orderItems;
            OrderStatus = orderStatus;
            Address = address;
            TotalCost = totalCost;
        }

        public void UpdateOrderStatus(OrderStatus confirmed)
        {
            OrderStatus = confirmed;
        }
    }

    public class Address
    {
        private string Street { get; set;  }
        private string City { get; set; }
        private string Country { get; set; }
        private string Postcode { get; set; }
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
