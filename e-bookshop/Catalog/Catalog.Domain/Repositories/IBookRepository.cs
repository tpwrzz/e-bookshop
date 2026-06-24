using Catalog.Domain.Enums;

namespace Catalog.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);

        Task<IEnumerable<Book>> GetAllAsync();

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task DeleteAsync(Guid id);
        Task<(IEnumerable<Book> books, int totalCount)> GetPagedAsync(
            int page,
            int pageSize,
            ICollection<Genre>? genre = null,
            bool? availability = null,
            int? rating = null,
            Language? language = null,
            string? authorName = null,
            CancellationToken cancellationToken = default); 
    }
}
