using Bookshop.SharedKernel.Domain.Enums;
using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = [];
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public string Address { get; set; } = string.Empty;
        public double TotalCost { get; set; }
        public Currency Currency { get; set; }
    }
}
