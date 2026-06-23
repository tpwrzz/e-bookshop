using Catalog.Domain;
using Catalog.Domain.Enums;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class BookRepository(CatalogContext context) : IBookRepository
    {
        private readonly CatalogContext _context = context;

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _context.Books
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

        public async Task<(IEnumerable<Book> books, int totalCount)> GetPagedAsync(int page, int pageSize, ICollection<Genres>? genre = null, bool? availability = null, int? rating = null, Languages? language = null, string? authorName = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (availability is not null)
                query = query.Where(b => b.Availability == availability);

            if (language is not null)
                query = query.Where(b => b.Language == language);

            if (rating is not null)
                query = query.Where(b => b.AverageRating >= rating);

            if (authorName is not null)
                query = query.Where(b => b.Author.FirstName.Contains(authorName)
                                       || b.Author.LastName.Contains(authorName));

            if (genre is not null && genre.Any())
                query = query.Where(b => b.Genre.Any(g => genre.Contains(g)));

            var totalCount = await query.CountAsync(cancellationToken);

            var books = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (books, totalCount);

        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
