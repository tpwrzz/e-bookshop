namespace Catalog.Application.DTOs.Auhtors
{
    public class AddAuthorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Bio { get; set; }
    }
}