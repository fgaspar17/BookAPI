using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IBookRepository _repository;
    public BooksController(AppDbContext context, IBookRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    // GET: <BooksController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookGetResponseDto>>> GetBooks(CancellationToken ct)
    {
        var books = await _repository.GetAllAsync(ct);

        if (books.Count() == 0)
            return Problem("There are no books", statusCode: 404);

        return Ok(books.Select(b => b.MapToDto()));
    }

    // GET <BooksController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id, CancellationToken ct)
    {
        var book = await _repository.GetByIdAsync(id, ct);

        if (book == null)
            return Problem($"There is no book wiht Id: {id}", statusCode: 404);

        return Ok(book.MapToDto());
    }

    // POST <BooksController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] BookPostRequestDto requestDto, CancellationToken ct)
    {
        var value = requestDto.MapToEntity();
        if (value == null)
            return BadRequest("No Book data provided.");

        await _repository.InsertAsync(value, ct);

        return CreatedAtAction(nameof(GetBook), new { id = value.BookId }, value);
    }

    // PUT <BooksController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] BookPutRequestDto requestDto, CancellationToken ct)
    {
        var value = requestDto.MapToEntity();
        if (value == null)
            return BadRequest("No Book data provided.");

        if (id != value.BookId)
            return BadRequest("Id isn't the same.");

        var book = await _context.Books.FindAsync([id], cancellationToken: ct);
        if (book == null)
            return Problem($"There is no book with Id: {id}", statusCode: 404);

        await _repository.UpdateAsync(value, ct);

        return NoContent();
    }

    // DELETE <BooksController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        var book = await _context.Books.FindAsync([id], cancellationToken: ct);
        if (book == null)
            return Problem($"There is no book with Id: {id}", statusCode: 404);

        await _repository.DeleteAsync(book, ct);

        return NoContent();
    }
}
