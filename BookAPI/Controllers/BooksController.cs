using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using BookAPI.Services.Result;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBooksService _service;

    public BooksController(AppDbContext context, IBooksRepository repository, IBooksService service)
    {
        _service = service;
    }

    // GET: api/Books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookResponseDTO>>> GetBooks(CancellationToken ct)
    {
        var serviceResult = await _service.GetAllBooksAsync(ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.NotFound => Problem(serviceResult.ErrorMessage, statusCode: 404),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return Ok(serviceResult.Data);
    }

    // GET api/Books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id, CancellationToken ct)
    {
        var serviceResult = await _service.GetBookByIdAsync(id, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.NotFound => Problem(serviceResult.ErrorMessage, statusCode: 404),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return Ok(serviceResult.Data);
    }

    // POST api/Books
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] BookPostRequestDto requestDto, CancellationToken ct)
    {
        var serviceResult = await _service.CreateBookAsync(requestDto, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.ValidationError => Problem(serviceResult.ErrorMessage, statusCode: 400),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return CreatedAtAction(nameof(GetBook), new { id = serviceResult.Data.BookId }, serviceResult.Data);
    }

    // PUT api/Books/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] BookPutRequestDto requestDto, CancellationToken ct)
    {
        var serviceResult = await _service.UpdateBookAsync(id, requestDto, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.ValidationError => Problem(serviceResult.ErrorMessage, statusCode: 422),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return NoContent();
    }

    // DELETE api/Books/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var serviceResult = await _service.DeleteBookAsync(id, ct);

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
