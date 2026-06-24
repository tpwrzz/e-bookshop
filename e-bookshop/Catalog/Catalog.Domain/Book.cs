using Bookshop.SharedKernel.Domain;
using Catalog.Domain.Enums;

namespace Catalog.Domain
{
    public class Book
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public string Description { get; private set; }
        public ICollection<Genre> Genre { get; private set; }
        public int PageCount { get; private set; }
        public Money Price { get; private set; }
        public Language Language { get; private set; }
        public double AverageRating { get; private set; }
        public bool Availability { get; private set; }
        public DateTime PublicationDate { get; private set; }
        public ICollection<Review> Reviews { get; private set; } = [];
        private Book() { }

        public Book(Guid id, string title, string description, ICollection<Genre> genre, int pageCount, Money price,
                    Language language, bool availability, DateTime publicationDate, Author author, ICollection<Review> reviews = null)
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
            Reviews = reviews is null ? [] : (ICollection<Review>)reviews.ToList();
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
        public void UpdateReview(Guid reviewId, Guid userId, string message, Rating rating)
        {
            var review = Reviews.FirstOrDefault(r => r.Id == reviewId) ?? throw new InvalidOperationException($"Review {reviewId} not found.");
            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only edit your own reviews.");

            review.Update(message, rating);
            AverageRating = Reviews.Average(r => r.Rating.Value);
        }
    }
}
