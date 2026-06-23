using e_bookshop.Catalog.Domain;
using e_bookshop.Domain.Enums;

namespace e_bookshop.Domain
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public string Description { get; private set; }
        public ICollection<Genres> Genre { get; private set; }
        public int PageCount { get; private set; }
        public Money Price { get; private set; }
        public Languages Language { get; private set; }
        public double AverageRating { get; private set; }
        public bool Availability { get; private set; }
        public DateTime PublicationDate { get; private set; }
        
        public ICollection<Review> Reviews { get; private set; } = [];

        public Book(Guid id, string title, string description, ICollection<Genres> genre, int pageCount, Money price,
                    Languages language, bool availability, DateTime publicationDate, Author author)
        {
            Id = id;
            Title = title;
            Description = description;
            Genre = genre;
            PageCount = pageCount;
            Price = price;
            Language = language;
            Availability = availability;
            PublicationDate = publicationDate;
            Author = author;
        }
        public void UpdatePrice(Money newPrice)
        {
            Price = newPrice;
        }
        public void AddReview(Guid userId, string message, Rating rating)
        {
            var review = new Review(
                id: Guid.NewGuid(),
                bookId: Id,
                userId: userId,
                message: message,
                rating: rating,
                createdAt: DateTime.UtcNow,
                updatedAt: DateTime.UtcNow
            );
            Reviews.Add(review);
            AverageRating = Reviews.Average(r => r.Rating.Value);
        }
    }
}
