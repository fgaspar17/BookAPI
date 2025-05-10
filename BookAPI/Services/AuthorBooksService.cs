using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Repositories;
using BookAPI.Services.Result;
using FluentValidation;

namespace BookAPI.Services;

public class AuthorBooksService : IAuthorBooksService
{
    private readonly IAuthorBooksRepository _repository;
    private readonly IValidator<AuthorBookPostRequestDto> _postValidator;

    public AuthorBooksService(IAuthorBooksRepository repository, IValidator<AuthorBookPostRequestDto> postValidator)
    {
        _repository = repository;
        _postValidator = postValidator;
    }

    public async Task<ResultService<AuthorBookResponseDTO>> CreateAuthorBookAsync(AuthorBookPostRequestDto requestDto, 
        CancellationToken ct)
    {
        var validationResult = _postValidator.Validate(requestDto);
        if (!validationResult.IsValid)
            return ResultService<AuthorBookResponseDTO>.FluentValidationFail(validationResult.Errors);

        var authorBook = requestDto.MapToEntity();

        await _repository.InsertAsync(authorBook, ct);
        var created = await _repository.GetAuthorBookByAuthorIdAndBookId(authorBook.AuthorId, authorBook.BookId, ct);

        return ResultService<AuthorBookResponseDTO>.Ok(created.MapToDto());
    }

    public async Task<ResultService<AuthorBookResponseDTO>> DeleteAuthorBookAsync(int authorId, int bookId, CancellationToken ct)
    {
        var authorBook = await _repository.GetAuthorBookByAuthorIdAndBookId(authorId, bookId, ct);
        if (authorBook == null)
            return ResultService<AuthorBookResponseDTO>.Fail($"There is no AuthorBook with AuthorId: {authorId} and BookId: {bookId}",
                ServiceErrorCode.NotFound);

        await _repository.DeleteAsync(authorBook, ct);

        return ResultService<AuthorBookResponseDTO>.Ok(data: null);
    }
}
