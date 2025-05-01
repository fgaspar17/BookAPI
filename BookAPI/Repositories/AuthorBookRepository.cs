using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories;

public class AuthorBookRepository : IAuthorBookRepository
{
    private readonly AppDbContext _context;
    public AuthorBookRepository(AppDbContext context)
    {
        _context = context;
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
