﻿using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories;

public class AuthorsRepository : IAuthorsRepository
{
    private readonly AppDbContext _context;
    public AuthorsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(Author value, CancellationToken ct)
    {
        _context.Authors.Remove(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }

    public IQueryable<Author> GetAllQuery()
    {
        return _context.Authors
            .AsNoTracking()
            .Include(author => author.AuthorBooks)
                .ThenInclude(ab => ab.Book);
    }

    public async Task<Author?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Authors
            .AsNoTracking()
            .Include(author => author.AuthorBooks)
                .ThenInclude(ab => ab.Book)
            .FirstOrDefaultAsync(a => a.AuthorId == id, cancellationToken: ct);
    }

    public async Task InsertAsync(Author value, CancellationToken ct)
    {
        _context.Authors.Add(value);
        await _context.SaveChangesAsync(cancellationToken: ct);
    }

    public async Task UpdateAsync(Author value, CancellationToken ct)
    {
        _context.Authors.Entry(value).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken: ct);
    }
}
