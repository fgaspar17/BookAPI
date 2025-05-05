using BookAPI.Models;

namespace BookAPI.Repositories;

public interface IAuthorBooksRepository
{
    public Task<AuthorBook?> GetAuthorBookByAuthorIdAndBookId(int authorId, int bookId, CancellationToken ct);
    public Task InsertAsync(AuthorBook value, CancellationToken ct);
    public Task DeleteAsync(AuthorBook value, CancellationToken ct);
}
