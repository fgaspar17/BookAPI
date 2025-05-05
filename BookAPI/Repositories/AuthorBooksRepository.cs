using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories;

public class AuthorBooksRepository : IAuthorBooksRepository
{
    private readonly AppDbContext _context;
    public AuthorBooksRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AuthorBook?> GetAuthorBookByAuthorIdAndBookId(int authorId, int bookId, CancellationToken ct)
    {
        return await _context.AuthorBooks
                .Where(ab => ab.AuthorId == authorId && ab.BookId == bookId)
                .SingleOrDefaultAsync();
    }

    public async Task DeleteAsync(AuthorBook value, CancellationToken ct)
    {
        _context.AuthorBooks.Remove(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }

    public async Task InsertAsync(AuthorBook value, CancellationToken ct)
    {
        _context.AuthorBooks.Add(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }
}
