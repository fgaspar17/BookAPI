using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthorsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Authors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors(CancellationToken ct)
    {
        var authors = await _context.Authors.ToListAsync(cancellationToken: ct);
        if (authors.Count == 0)
            return Problem("There are no authors", statusCode: 404);

        return Ok(authors);
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthor(int id, CancellationToken ct)
    {
        var author = await _context.Authors.FindAsync([id], cancellationToken: ct);

        if (author == null)
            return Problem($"There is no author wiht Id: {id}", statusCode: 404);

        return Ok(author);
    }

    // PUT: api/Authors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(int id, Author value, CancellationToken ct)
    {
        if (id != value.AuthorId)
            return BadRequest("Id isn't the same.");

        var author = await _context.Authors.FindAsync([id], cancellationToken: ct);
        if (author == null)
            return Problem($"There is no author with Id: {id}", statusCode: 404);

        author.FirstName = value.FirstName;
        author.LastName = value.LastName;
        author.Birthday = value.Birthday;

        await _context.SaveChangesAsync(cancellationToken: ct);

        return NoContent();
    }

    // POST: api/Authors
    [HttpPost]
    public async Task<ActionResult<Author>> PostAuthor(Author author, CancellationToken ct)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync(cancellationToken: ct);

        return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, author);
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id, CancellationToken ct)
    {
        var author = await _context.Authors.FindAsync([id], cancellationToken: ct);

        if (author == null)
            return Problem($"There is no author with Id: {id}", statusCode: 404);

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync(cancellationToken: ct);

        return NoContent();
    }
}
