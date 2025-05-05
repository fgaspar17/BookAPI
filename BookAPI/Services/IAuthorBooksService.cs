using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Services.Result;

namespace BookAPI.Services;

public interface IAuthorBooksService
{
    Task<ResultService<AuthorBookResponseDTO>> CreateAuthorBookAsync(AuthorBookPostRequestDto requestDto, CancellationToken ct);
    Task<ResultService<AuthorBookResponseDTO>> DeleteAuthorBookAsync(int authorId, int bookId, CancellationToken ct);
}
