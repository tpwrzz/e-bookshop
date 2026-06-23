using e_bookshop.Catalog.Application.DTOs.Reviews;
using e_bookshop.Domain.Enums;

namespace e_bookshop.Catalog.Application.DTOs.Books
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = "No summary yet.";
        public ICollection<Genres> Genre { get; set; } = [];
        public int PageCount { get; set; }
        public double Price { get; set; }
        public required string Currency { get; set; }
        public Languages Language { get; set; }
        public double AverageRating { get; set; }
        public bool Availability { get; set; }
        public required string PublicationDate { get; set; }
        public required AuthorDto Author { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; } = [];
    }
}
