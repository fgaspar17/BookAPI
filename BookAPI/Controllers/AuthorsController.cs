using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Mappers;
using BookAPI.Models;
using BookAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAuthorRepository _repository;

    public AuthorsController(AppDbContext context, IAuthorRepository repository)
    {
        _context = context;
        _repository = repository;

    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorGetResponseDto>>> GetAuthors(CancellationToken ct)
    {
        var authors = await _repository.GetAllAsync(ct);
        if (authors.Count() == 0)
            return Problem("There are no authors", statusCode: 404);

        return Ok(authors.Select(author => author.MapToDto()));
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorGetResponseDto>> GetAuthor(int id, CancellationToken ct)
    {
        var author = await _repository.GetByIdAsync(id, ct);

        if (author == null)
            return Problem($"There is no author wiht Id: {id}", statusCode: 404);

        return Ok(author.MapToDto());
    }

    // PUT: api/Authors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(int id, AuthorPutRequestDto requestDto, CancellationToken ct)
    {
        var value = requestDto.MapToEntity();
        if (value == null)
            return BadRequest("No Author data provided.");

        if (id != value.AuthorId)
            return BadRequest("Id isn't the same.");

        var author = await _context.Authors.FindAsync([id], cancellationToken: ct);
        if (author == null)
            return Problem($"There is no author with Id: {id}", statusCode: 404);

        await _repository.UpdateAsync(value, ct);

        return NoContent();
    }

    // POST: api/Authors
    [HttpPost]
    public async Task<ActionResult<Author>> PostAuthor(AuthorPostRequestDto authorRequest, CancellationToken ct)
    {
        var value = authorRequest.MapToEntity();
        if (value == null)
            return BadRequest();

        await _repository.InsertAsync(value, ct);

        return CreatedAtAction(nameof(GetAuthor), new { id = value.AuthorId }, value);
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id, CancellationToken ct)
    {
        var author = await _context.Authors.FindAsync([id], cancellationToken: ct);

        if (author == null)
            return Problem($"There is no author with Id: {id}", statusCode: 404);

        await _repository.DeleteAsync(author, ct);

        return NoContent();
    }
}
