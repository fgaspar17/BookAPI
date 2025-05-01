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
using BookAPI.Repositories;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorBooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthorBookRepository _repository;

        public AuthorBooksController(AppDbContext context, IAuthorBookRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // POST: api/AuthorBooks
        [HttpPost]
        public async Task<ActionResult<AuthorBook>> PostAuthorBook(AuthorBookPostRequestDto authorBookRequest, CancellationToken ct)
        {
            var authorBook = authorBookRequest.MapToEntity();
            if (authorBook is null)
                return BadRequest("No AuthorBook data provided.");

            await _repository.InsertAsync(authorBook, ct);

            return Created();
        }

        // DELETE: api/AuthorBooks/5
        [HttpDelete("{authorId} {bookId}")]
        public async Task<IActionResult> DeleteAuthorBook(int authorId, int bookId, CancellationToken ct)
        {
            var authorBook = await _context.AuthorBooks
                .Where(ab => ab.AuthorId == authorId && ab.BookId == bookId)
                .SingleOrDefaultAsync();

            if (authorBook == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(authorBook, ct);

            return NoContent();
        }
    }
}
