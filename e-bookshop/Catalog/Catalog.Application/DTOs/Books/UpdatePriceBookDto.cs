namespace Catalog.Application.DTOs.Books
{
    public class UpdatePriceBookDto
    {
        public Guid Id { get; set;}
        public decimal NewPrice { get; set;}
        public required string NewCurrency { get; set; }

    }
}