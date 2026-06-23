namespace e_bookshop.Catalog.Application.DTOs
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
    }
}