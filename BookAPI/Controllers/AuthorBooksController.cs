using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using BookAPI.DTOs.RequestDTOs;
using BookAPI.Mappers;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorBooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthorBooksController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/AuthorBooks
        [HttpPost]
        public async Task<ActionResult<AuthorBook>> PostAuthorBook(AuthorBookPostRequestDto authorBookRequest, CancellationToken ct)
        {
            var authorBook = authorBookRequest.MapToEntity();
            if (authorBook is null)
                return BadRequest("No AuthorBook data provided.");

            _context.AuthorBooks.Add(authorBook);
            await _context.SaveChangesAsync(cancellationToken: ct);

            return Created();
        }

        // DELETE: api/AuthorBooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorBook(int id, CancellationToken ct)
        {
            var authorBook = await _context.AuthorBooks.FindAsync([id], cancellationToken: ct);
            if (authorBook == null)
            {
                return NotFound();
            }

            _context.AuthorBooks.Remove(authorBook);
            await _context.SaveChangesAsync(cancellationToken: ct);

            return NoContent();
        }
    }
}
