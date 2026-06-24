using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.DTOs
{
    public class AddressDto
    {
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string Postcode { get; set; }
        
    }
}
