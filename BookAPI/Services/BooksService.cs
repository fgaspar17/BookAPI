using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services.Result;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace BookAPI.Services;

public class BooksService : IBooksService
{
    private readonly IBooksRepository _repository;

    public BooksService(IBooksRepository repository)
    {
        _repository = repository;
    }
    public async Task<ResultService<BookResponseDTO>> CreateBookAsync(BookPostRequestDto requestDto, CancellationToken ct)
    {
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

    public async Task<ResultService<IEnumerable<BookResponseDTO>>> GetAllBooksAsync(CancellationToken ct)
    {
        var books = await _repository.GetAllAsync(ct);

        if (!books.Any())
            return ResultService<IEnumerable<BookResponseDTO>>.Fail("There are no books",
                ServiceErrorCode.NotFound);

        return ResultService<IEnumerable<BookResponseDTO>>.Ok(books.Select(b => b.MapToDto())!);
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
}
