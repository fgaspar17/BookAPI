using BookAPI.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedDataController : ControllerBase
{
    private readonly AppDbContext _context;

    public SeedDataController(AppDbContext context)
    {
        _context = context;
    }


    // POST api/<SeedDataController>/5
    [HttpPost("seed")]
    public async Task<ActionResult> Post(CancellationToken ct)
    {
        if (_context.Books.Any() || _context.Authors.Any())
            return BadRequest("Database contains data");

        var books = new List<Book>();

        books.Add(new Book
        {
            BookId = 1,
            Title = "Mistborn: The Final Empire",
            Description = "Mistborn: The Final Empire, also known simply as Mistborn or The Final Empire, is a fantasy novel written by American author Brandon Sanderson.",
            PublicationDate = new DateTime(2006, 7, 17),
            Pages = 541,
        });

        books.Add(new Book
        {
            BookId = 2,
            Title = "A Mind for Numbers",
            Description = null,
            PublicationDate = new DateTime(2014, 7, 31),
            Pages = 336,
        });

        await _context.Books.AddRangeAsync(books, cancellationToken: ct);

        var authors = new List<Author>();

        authors.Add(new Author
        {
            AuthorId = 1,
            FirstName = "Brandon",
            LastName = "Sanderson",
            Birthday = new DateTime(1975, 12, 19)
        });

        authors.Add(new Author
        {
            AuthorId = 2,
            FirstName = "Barbara",
            LastName = "Oakley",
            Birthday = new DateTime(1955, 11, 24)
        });

        await _context.Authors.AddRangeAsync(authors, cancellationToken: ct);

        var authorBooks = new List<AuthorBook>();

        authorBooks.Add(new AuthorBook
        {
            AuthorId = 1,
            BookId = 1,
        });

        authorBooks.Add(new AuthorBook
        {
            AuthorId = 2,
            BookId = 2,
        });

        await _context.AuthorBooks.AddRangeAsync(authorBooks, cancellationToken: ct);

        await _context.SaveChangesAsync(cancellationToken: ct);

        return Ok("Database seeded successfully");
    }
}
