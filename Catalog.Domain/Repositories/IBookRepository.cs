using e_bookshop.Catalog.Application.DTOs.Books;
using e_bookshop.Domain;

namespace e_bookshop.Catalog.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);

        Task<IEnumerable<Book>> GetAllAsync();

        Task AddAsync(Book book);

        Task UpdateAsync(Book book);

        Task DeleteAsync(Guid id);
        Task<(IEnumerable<Book> books, int totalCount)> GetPagedAsync(BookFilterDto filter, CancellationToken cancellationToken);
    }
}
