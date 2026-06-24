using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.DTOs
{
    public class AddOrderItemDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
