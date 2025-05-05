using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Services.Result;

namespace BookAPI.Services;

public interface IBooksService
{
    Task<ResultService<IEnumerable<BookResponseDTO>>> GetAllBooksAsync(CancellationToken ct);
    Task<ResultService<BookResponseDTO>> GetBookByIdAsync(int id, CancellationToken ct);
    Task<ResultService<BookResponseDTO>> CreateBookAsync(BookPostRequestDto requestDto, CancellationToken ct);
    Task<ResultService<BookResponseDTO>> UpdateBookAsync(int bookId, BookPutRequestDto requestDto, CancellationToken ct);
    Task<ResultService<BookResponseDTO>> DeleteBookAsync(int bookId, CancellationToken ct);
}
