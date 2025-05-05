using BookAPI.Models;

namespace BookAPI.Repositories;

public interface IAuthorsRepository
{
    public Task<Author?> GetByIdAsync(int id, CancellationToken ct);
    public Task<IEnumerable<Author>> GetAllAsync(CancellationToken ct);
    public Task InsertAsync(Author value, CancellationToken ct);
    public Task UpdateAsync(Author value, CancellationToken ct);
    public Task DeleteAsync(Author value, CancellationToken ct);
}
