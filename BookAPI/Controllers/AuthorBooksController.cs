using BookAPI.DTOs.RequestDTOs;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using BookAPI.Services.Result;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorBooksController : ControllerBase
{
    private readonly IAuthorBooksService _service;

    public AuthorBooksController(AppDbContext context, IAuthorBooksRepository repository, IAuthorBooksService service)
    {
        _service = service;
    }

    // POST: api/AuthorBooks
    [HttpPost]
    public async Task<ActionResult<AuthorBook>> PostAuthorBook(AuthorBookPostRequestDto requestDto, CancellationToken ct)
    {
        var serviceResult = await _service.CreateAuthorBookAsync(requestDto, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.FluentValidationError => BadRequest(serviceResult.FluentValidationErrors.Select(e => new { e.PropertyName, e.ErrorMessage })),
                ServiceErrorCode.ValidationError => Problem(serviceResult.ErrorMessage, statusCode: 400),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return Created();
    }

    // DELETE: api/AuthorBooks/5
    [HttpDelete("authorId={authorId}&bookId={bookId}")]
    public async Task<IActionResult> DeleteAuthorBook(int authorId, int bookId, CancellationToken ct)
    {
        var serviceResult = await _service.DeleteAuthorBookAsync(authorId, bookId, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.NotFound => Problem(serviceResult.ErrorMessage, statusCode: 404),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return NoContent();
    }
}
