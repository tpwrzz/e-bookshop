using e_bookshop.Catalog.Domain;
using e_bookshop.Catalog.Domain.Repositories;
using e_bookshop.Catalog.Infrastructure;
using e_bookshop.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly CatalogContext _context;

        public BookRepository(CatalogContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        { var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id);
            _ = _context.Remove(book);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
            .Include(b => b.Author).ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<(IEnumerable<Book> books, int totalCount)> GetPagedAsync(int page, int pageSize, ICollection<Genres>? genre = null, bool? availability = null, int? rating = null, Languages? language = null, string? authorName = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
