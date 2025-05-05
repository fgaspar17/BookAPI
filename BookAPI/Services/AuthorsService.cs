using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Repositories;
using BookAPI.Services.Result;

namespace BookAPI.Services;

public class AuthorsService : IAuthorsService
{
    private readonly IAuthorsRepository _repository;

    public AuthorsService(IAuthorsRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResultService<AuthorResponseDTO>> CreateAuthorAsync(AuthorPostRequestDto requestDto, CancellationToken ct)
    {
        var value = requestDto.MapToEntity();

        await _repository.InsertAsync(value, ct);
        var created = await _repository.GetByIdAsync(value.AuthorId, ct);

        return ResultService<AuthorResponseDTO>.Ok(created.MapToDto());
    }

    public async Task<ResultService<AuthorResponseDTO>> DeleteAuthorAsync(int authorId, CancellationToken ct)
    {
        var author = await _repository.GetByIdAsync(authorId, ct);
        if (author == null)
            return ResultService<AuthorResponseDTO>.Fail($"There is no author with Id: {authorId}",
                ServiceErrorCode.NotFound);

        await _repository.DeleteAsync(author, ct);

        return ResultService<AuthorResponseDTO>.Ok(data: null);
    }

    public async Task<ResultService<IEnumerable<AuthorResponseDTO>>> GetAllAuthorsAsync(CancellationToken ct)
    {
        var authors = await _repository.GetAllAsync(ct);
        if (!authors.Any())
            return ResultService<IEnumerable<AuthorResponseDTO>>.Fail("There are no authors",
                ServiceErrorCode.NotFound);

        return ResultService<IEnumerable<AuthorResponseDTO>>.Ok(authors.Select(author => author.MapToDto())!);
    }

    public async Task<ResultService<AuthorResponseDTO>> GetAuthorByIdAsync(int authorId, CancellationToken ct)
    {
        var author = await _repository.GetByIdAsync(authorId, ct);
        if (author == null)
            return ResultService<AuthorResponseDTO>.Fail($"There is no author wiht Id: {authorId}",
                ServiceErrorCode.NotFound);

        return ResultService<AuthorResponseDTO>.Ok(author.MapToDto()!);
    }

    public async Task<ResultService<AuthorResponseDTO>> UpdateAuthorAsync(int authorId, AuthorPutRequestDto requestDto, CancellationToken ct)
    {
        var value = requestDto.MapToEntity();

        if (authorId != value.AuthorId)
            return ResultService<AuthorResponseDTO>.Fail("Request ID does not match entity ID.",
                ServiceErrorCode.ValidationError);

        var author = await _repository.GetByIdAsync(authorId, ct);
        if (author == null)
            return ResultService<AuthorResponseDTO>.Fail($"There is no author wiht Id: {authorId}",
                ServiceErrorCode.NotFound);

        await _repository.UpdateAsync(value, ct);
        var updated = await _repository.GetByIdAsync(value.AuthorId, ct);

        return ResultService<AuthorResponseDTO>.Ok(updated.MapToDto());
    }
}
