using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;
    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: <BooksController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _context.Books.ToListAsync();
        if (books.Count == 0)
            return Problem("There are no books", statusCode: 404);

        return Ok(books);
    }

    // GET <BooksController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return Problem($"There is no book wiht Id: {id}", statusCode: 404);

        return Ok(book);
    }

    // POST <BooksController>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] Book value)
    {
        await _context.Books.AddAsync(value);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBook), new { id = value.BookId }, value);
    }

    // PUT <BooksController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] Book value)
    {
        if (id != value.BookId)
            return BadRequest("Id isn't the same.");

        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return Problem($"There is no book with Id: {id}", statusCode: 404);

        book.Title = value.Title;
        book.Description = value.Description;
        book.Pages = value.Pages;
        book.PublicationDate = value.PublicationDate;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE <BooksController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
            return Problem($"There is no book with Id: {id}", statusCode: 404);

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
