using Bookshop.SharedKernel.Domain.Common.Enums;
using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = [];
        public DateTime PlacedDate { get; set; }
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public required AddressDto Address { get; set; }
        public decimal TotalCost { get; set; }
        public Currency Currency { get; set; }
        public Guid UserId { get; set; }
    }
}
