using Catalog.Application.DTOs.Pagination;
using Catalog.Domain.Enums;

namespace Catalog.Application.DTOs.Books
{
    public class BookFilterDto
    {
        public ICollection<Genre>? Genre { get; set; } = null;
        public bool? Availability { get; set; } =null;
        public int? Rating { get; set; } = null;
        public Language? Language { get; set; } = null;
        public string? AuthorName { get; set; } = null;
        public PaginationDto Pagination { get; set; } = new PaginationDto();
    }
}