using System.Linq.Expressions;
using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services.Result;
using BookAPI.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Services;

public class AuthorsService : IAuthorsService
{
    private readonly IAuthorsRepository _repository;
    private readonly IValidator<AuthorPostRequestDto> _postValidator;
    private readonly IValidator<AuthorPutRequestDto> _putValidator;
    private readonly AuthorQueryParametersValidator _queryParamsValidator;

    public AuthorsService(IAuthorsRepository repository, IValidator<AuthorPostRequestDto> postValidator,
        IValidator<AuthorPutRequestDto> putValidator, AuthorQueryParametersValidator queryParamsValidator)
    {
        _repository = repository;
        _postValidator = postValidator;
        _putValidator = putValidator;
        _queryParamsValidator = queryParamsValidator;
    }

    public async Task<ResultService<AuthorResponseDTO>> CreateAuthorAsync(AuthorPostRequestDto requestDto, CancellationToken ct)
    {
        var validationResult = _postValidator.Validate(requestDto);
        if (!validationResult.IsValid)
            return ResultService<AuthorResponseDTO>.FluentValidationFail(validationResult.Errors);

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

    public async Task<ResultService<PagedList<AuthorResponseDTO>>> GetAllAuthorsAsync(GetAllQueryParameters parameters, CancellationToken ct)
    {
        var validationResult = _queryParamsValidator.Validate(parameters);
        if (!validationResult.IsValid)
            return ResultService<PagedList<AuthorResponseDTO>>.FluentValidationFail(validationResult.Errors);

        var query = _repository.GetAllQuery();

        // Filtering
        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            query = query
                .Where(a => EF.Functions.Like(a.FirstName, $"%{parameters.Filter}%"));
        }

        // Sortering
        if (parameters.SortDirection.ToLower() == "desc")
            query = query
                .OrderByDescending(GetSortProperty(parameters.SortColumn));
        else
            query = query
                .OrderBy(GetSortProperty(parameters.SortColumn));

        // Mapping to DTO
        var queryDto = query.Select(a => a.MapToDto());

        // Pagination
        var pagedAuthors = await PagedList<AuthorResponseDTO>.CreateAsync(queryDto, parameters.PageIndex, parameters.PageSize, ct);

        if (pagedAuthors.Data.Count == 0)
            return ResultService<PagedList<AuthorResponseDTO>>.Fail("There are no authors",
                ServiceErrorCode.NotFound);

        return ResultService<PagedList<AuthorResponseDTO>>.Ok(pagedAuthors);
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
        var validationResult = _putValidator.Validate(requestDto);
        if (!validationResult.IsValid)
            return ResultService<AuthorResponseDTO>.FluentValidationFail(validationResult.Errors);

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

    private Expression<Func<Author, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "firstname" => author => author.FirstName,
            "lastname" => author => author.LastName,
            "birthday" => author => author.Birthday,
            _ => author => author.AuthorId
        };
    }
}
