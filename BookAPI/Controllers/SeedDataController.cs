using BookAPI.Constants;
using BookAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = RoleNames.Administrator)]
public class SeedDataController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<SeedDataController> _logger;

    public SeedDataController(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager,
        ILogger<SeedDataController> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }


    // POST api/<SeedDataController>/5
    [HttpPost]
    public async Task<ActionResult> Data(CancellationToken ct)
    {
        if (_context.Books.Any() || _context.Authors.Any())
        {
            _logger.LogWarning("Database seeding aborted: database already contains data.");
            return BadRequest("Database contains data");
        }

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

        _logger.LogInformation("Seeded {bookCount} books, {authorCount} authors, and {linkCount} author-book relations.", books.Count, authors.Count, authorBooks.Count);
        return Ok("Database seeded successfully.");
    }

    [HttpPost]
    public async Task<ActionResult> AuthData(CancellationToken ct)
    {

        if (!await _roleManager.RoleExistsAsync(RoleNames.Administrator))
        {
            var role = new IdentityRole(RoleNames.Administrator);
            await _roleManager.CreateAsync(role);
        }
        else
        {
            _logger.LogWarning("Role '{roleName}' already exists. Skipping creation.", RoleNames.Administrator);
            return BadRequest("Roles have been already created.");
        }

        _logger.LogInformation("Role '{roleName}' created successfully.", RoleNames.Administrator);
        return Ok("Roles seeded successfully.");
    }

    [HttpPatch]
    public async Task<ActionResult> AssignRole(string userName, string roleName, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (await _roleManager.RoleExistsAsync(roleName) && user != null)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
        else
        {
            _logger.LogWarning("The User: {userName} or the Role: {roleName} don't exist.", userName, roleName);
            return BadRequest("The User or the Role doesn't exist.");
        }

        _logger.LogInformation("Roles assigned successfully.");
        return Ok("Roles assigned successfully. Authorize this endpoint");
    }
}
