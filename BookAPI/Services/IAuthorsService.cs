using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;
using BookAPI.Services.Result;

namespace BookAPI.Services;

public interface IAuthorsService
{
    Task<ResultService<PagedList<AuthorResponseDTO>>> GetAllAuthorsAsync(GetAllQueryParameters parameters, CancellationToken ct);
    Task<ResultService<AuthorResponseDTO>> GetAuthorByIdAsync(int authorId, CancellationToken ct);
    Task<ResultService<AuthorResponseDTO>> CreateAuthorAsync(AuthorPostRequestDto requestDto, CancellationToken ct);
    Task<ResultService<AuthorResponseDTO>> UpdateAuthorAsync(int authorId, AuthorPutRequestDto requestDto, CancellationToken ct);
    Task<ResultService<AuthorResponseDTO>> DeleteAuthorAsync(int authorId, CancellationToken ct);
}
