namespace Catalog.Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid id);

        Task<IEnumerable<Author>> GetAllAsync();

        Task AddAsync(Author author);

        Task UpdateAsync(Author author);

        Task DeleteAsync(Guid id);
    }
}