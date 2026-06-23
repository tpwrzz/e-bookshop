namespace e_bookshop.Catalog.Application.DTOs.Books
{
    public class UpdatePriceBookDto
    {
        public Guid Id { get; set;}
        public double NewPrice { get; set;}
        public required string NewCurrency { get; set; }

    }
}