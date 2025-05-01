using BookAPI.Models;

namespace BookAPI.Repositories;

public interface IAuthorBookRepository
{
    public Task InsertAsync(AuthorBook value, CancellationToken ct);
    public Task DeleteAsync(AuthorBook value, CancellationToken ct);
}
