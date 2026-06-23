namespace e_bookshop.Domain
{
    public class Review
    {
        public Guid Id { get; private set; }
        public Guid BookId { get; private set; }
        public Guid UserId { get; private set; }
        public string Message { get; private set; }
        public Rating Rating { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public Review(Guid id, Guid bookId, Guid userId, string message, Rating rating, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            BookId = bookId;
            UserId = userId;
            Message = message;
            Rating = rating;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
    public sealed class Rating
    {
        public int Value { get; }

        public Rating(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Rating must be between 1 and 5");

            Value = value;
        }
    }
}
