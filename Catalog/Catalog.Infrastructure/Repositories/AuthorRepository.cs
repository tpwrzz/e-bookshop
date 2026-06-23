using e_bookshop.Catalog.Domain;
using e_bookshop.Catalog.Domain.Repositories;

namespace Catalog.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        public Task AddAsync(Author book)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Author>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Author?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Author book)
        {
            throw new NotImplementedException();
        }
    }
}
