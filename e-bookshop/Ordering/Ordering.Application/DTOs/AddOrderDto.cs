using Bookshop.SharedKernel.Domain.Common.Enums;
using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public class AddOrderDto
    {
        public ICollection<AddOrderItemDto> OrderItems { get; set; } = [];
        public required AddressDto Address { get; set; } 
        public Currency Currency { get; set; }
        public Guid UserId { get; set; }
    }
}
