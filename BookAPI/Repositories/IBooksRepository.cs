using BookAPI.Models;

namespace BookAPI.Repositories;

public interface IBooksRepository
{
    public Task<Book?> GetByIdAsync(int id, CancellationToken ct);
    public IQueryable<Book> GetAllQuery();
    public Task InsertAsync(Book value, CancellationToken ct);
    public Task UpdateAsync(Book value, CancellationToken ct);
    public Task DeleteAsync(Book value, CancellationToken ct);
}
