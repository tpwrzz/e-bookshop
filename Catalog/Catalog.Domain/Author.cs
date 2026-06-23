namespace Catalog.Domain
{
    public class Author
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Bio { get; private set; }
        private Author() { }

        public Author(Guid id,  string firstName, string lastName, string? bio)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Bio = bio;
        }
    }
}
