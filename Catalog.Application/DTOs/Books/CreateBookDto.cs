using e_bookshop.Domain.Enums;

namespace e_bookshop.Catalog.Application.DTOs.Books
{
    public class CreateBookDto
    {
        public required string Title { get; set; }
        public string Description { get; set; } = "No summary yet.";
        public ICollection<Genres> Genre { get; set; } = [];
        public int PageCount { get; set; }
        public double Price { get; set; }
        public Currencies Currency { get; set; }
        public Languages Language { get; set; }
        public double AverageRating { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid AuthorId { get; set; }
    }
}
