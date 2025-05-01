using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;
    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(ab => ab.BookAuthors)
                .ThenInclude(a => a.Author)
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Books
            .AsNoTracking()
            .Include(ab => ab.BookAuthors)
                .ThenInclude(a => a.Author)
            .FirstOrDefaultAsync(b => b.BookId == id, cancellationToken: ct);
    }

    public async Task InsertAsync(Book value, CancellationToken ct)
    {
        _context.Books.Add(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }

    public async Task UpdateAsync(Book value, CancellationToken ct)
    {
        _context.Entry(value).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken: ct);
    }

    public async Task DeleteAsync(Book value, CancellationToken ct)
    {
        _context.Books.Remove(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }
}
