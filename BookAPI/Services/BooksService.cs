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

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;
    private readonly IValidator<BookPostRequestDto> _postValidator;
    private readonly IValidator<BookPutRequestDto> _putValidator;
    private readonly BookQueryParametersValidator _queryParamsValidator;

    public BooksService(IBooksRepository repository, IValidator<BookPostRequestDto> postValidator, 
        IValidator<BookPutRequestDto> putValidator, BookQueryParametersValidator queryParamsValidator)
    {
        _repository = repository;
        _postValidator = postValidator;
        _putValidator = putValidator;
        _queryParamsValidator = queryParamsValidator;
    }
    public async Task<ResultService<BookResponseDTO>> CreateBookAsync(BookPostRequestDto requestDto, CancellationToken ct)
    {
        var validationResult = _postValidator.Validate(requestDto);
        if (!validationResult.IsValid)
            return ResultService<BookResponseDTO>.FluentValidationFail(validationResult.Errors);

        var value = requestDto.MapToEntity();

        await _repository.InsertAsync(value, ct);

        var created = await _repository.GetByIdAsync(value.BookId, ct);

        return ResultService<BookResponseDTO>.Ok(created.MapToDto());
    }

    public async Task<ResultService<BookResponseDTO>> DeleteBookAsync(int bookId, CancellationToken ct)
    {
        var book = await _repository.GetByIdAsync(bookId, ct);
        if (book == null)
            return ResultService<BookResponseDTO>.Fail($"There is no book with Id: {bookId}",
                ServiceErrorCode.NotFound);

        await _repository.DeleteAsync(book, ct);

        return ResultService<BookResponseDTO>.Ok(data: null);
    }

    public async Task<ResultService<PagedList<BookResponseDTO>>> GetAllBooksAsync(GetAllQueryParameters parameters, CancellationToken ct)
    {
        var validationResult = _queryParamsValidator.Validate(parameters);
        if (!validationResult.IsValid)
            return ResultService<PagedList<BookResponseDTO>>.FluentValidationFail(validationResult.Errors);

        var query = _repository.GetAllQuery();

        // Filtering
        if (!string.IsNullOrEmpty(parameters.Filter))
        {
            query = query
                .Where(b => EF.Functions.Like(b.Title, $"%{parameters.Filter}%"));
        }

        // Sortering
        if (parameters.SortDirection.ToLower() == "desc")
            query = query
                .OrderByDescending(GetSortProperty(parameters.SortColumn));
        else
            query = query
                .OrderBy(GetSortProperty(parameters.SortColumn));

        // Mapping to DTO
        var queryDto = query.Select(b => b.MapToDto());

        // Pagination
        var pagedBooks = await PagedList<BookResponseDTO>.CreateAsync(queryDto, parameters.PageIndex, parameters.PageSize, ct);

        if (pagedBooks.Data.Count == 0)
            return ResultService<PagedList<BookResponseDTO>>.Fail("There are no books",
                ServiceErrorCode.NotFound);

        return ResultService<PagedList<BookResponseDTO>>.Ok(pagedBooks!);
    }

    public async Task<ResultService<BookResponseDTO>> GetBookByIdAsync(int id, CancellationToken ct)
    {
        var book = await _repository.GetByIdAsync(id, ct);
        if (book == null)
            return ResultService<BookResponseDTO>.Fail($"There is no book wiht Id: {id}",
                ServiceErrorCode.NotFound);

        return ResultService<BookResponseDTO>.Ok(book.MapToDto()!);
    }

    public async Task<ResultService<BookResponseDTO>> UpdateBookAsync(int bookId, BookPutRequestDto requestDto, CancellationToken ct)
    {
        var validationResult = _putValidator.Validate(requestDto);
        if (!validationResult.IsValid)
            return ResultService<BookResponseDTO>.FluentValidationFail(validationResult.Errors);

        var value = requestDto.MapToEntity();

        if (bookId != value.BookId)
            return ResultService<BookResponseDTO>.Fail("Request ID does not match entity ID.",
               ServiceErrorCode.ValidationError);

        var book = await _repository.GetByIdAsync(bookId, ct);
        if (book == null)
            return ResultService<BookResponseDTO>.Fail($"There is no book with Id: {bookId}",
                ServiceErrorCode.NotFound);

        await _repository.UpdateAsync(value, ct);

        var updated = await _repository.GetByIdAsync(value.BookId, ct);

        return ResultService<BookResponseDTO>.Ok(updated.MapToDto());
    }

    private Expression<Func<Book, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "publicationdate" => book => book.PublicationDate,
            "pages" => book => book.Pages,
            "title" => book => book.Title,
            _ => book => book.BookId
        };
    }
}
