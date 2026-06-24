using Bookshop.SharedKernel.Domain.Enums;
using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Books
{
    public class CreateBookDto
    {
        public required string Title { get; set; }
        public string Description { get; set; } = "No summary yet.";
        public ICollection<Genre> Genre { get; set; } = [];
        public int PageCount { get; set; }
        public double Price { get; set; }
        public Currency Currency { get; set; }
        public Language Language { get; set; }
        public double AverageRating { get; set; }
        public DateTime PublicationDate { get; set; }
        public Guid AuthorId { get; set; }
    }
}
