namespace Catalog.Application.DTOs.Pagination
{
    public class PaginationDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}