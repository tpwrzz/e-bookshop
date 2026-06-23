namespace e_bookshop.Catalog.Domain
{
    public class Author
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Bio { get; private set; }
        public Author() { }
    }
}
