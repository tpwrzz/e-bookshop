namespace e_bookshop.Catalog.Domain.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author?> GetByIdAsync(Guid id);

        Task<IEnumerable<Author>> GetAllAsync();

        Task AddAsync(Author book);

        Task UpdateAsync(Author book);

        Task DeleteAsync(Guid id);
    }
}