using BookAPI.Models;

namespace BookAPI.Repositories;

public interface IBooksRepository
{
    public Task<Book?> GetByIdAsync(int id, CancellationToken ct);
    public Task<IEnumerable<Book>> GetAllAsync(CancellationToken ct);
    public Task InsertAsync(Book value, CancellationToken ct);
    public Task UpdateAsync(Book value, CancellationToken ct);
    public Task DeleteAsync(Book value, CancellationToken ct);
}
