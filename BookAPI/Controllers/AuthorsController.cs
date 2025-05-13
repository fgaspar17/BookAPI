using BookAPI.Constants;
using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;
using BookAPI.Repositories;
using BookAPI.Services;
using BookAPI.Services.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = RoleNames.Administrator)]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorsService _service;

    public AuthorsController(AppDbContext context, IAuthorsService service, IAuthorsRepository repository)
    {
        _service = service;
    }

    // GET: api/Authors
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedList<AuthorResponseDTO>>> GetAuthors([FromQuery] GetAllQueryParameters parameters, 
        CancellationToken ct)
    {
        var serviceResult = await _service.GetAllAuthorsAsync(parameters, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.FluentValidationError => BadRequest(serviceResult.FluentValidationErrors.Select(e => new { e.PropertyName, e.ErrorMessage })),
                ServiceErrorCode.NotFound => Problem(serviceResult.ErrorMessage, statusCode: 404),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return Ok(serviceResult.Data);
    }

    // GET: api/Authors/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthorResponseDTO>> GetAuthor(int id, CancellationToken ct)
    {
        var serviceResult = await _service.GetAuthorByIdAsync(id, ct);

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

    // POST: api/Authors
    [HttpPost]
    public async Task<ActionResult<Author>> PostAuthor(AuthorPostRequestDto requestDto, CancellationToken ct)
    {
        var serviceResult = await _service.CreateAuthorAsync(requestDto, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.FluentValidationError => BadRequest(serviceResult.FluentValidationErrors.Select(e => new { e.PropertyName, e.ErrorMessage})),
                ServiceErrorCode.ValidationError => Problem(serviceResult.ErrorMessage, statusCode: 400),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return CreatedAtAction(nameof(GetAuthor), new { id = serviceResult.Data.AuthorId }, serviceResult.Data);
    }

    // PUT: api/Authors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuthor(int id, AuthorPutRequestDto requestDto, CancellationToken ct)
    {
        var serviceResult = await _service.UpdateAuthorAsync(id, requestDto, ct);

        if (!serviceResult.Success)
        {
            return serviceResult.ErrorCode switch
            {
                ServiceErrorCode.FluentValidationError => BadRequest(serviceResult.FluentValidationErrors.Select(e => new { e.PropertyName, e.ErrorMessage })),
                ServiceErrorCode.ValidationError => Problem(serviceResult.ErrorMessage, statusCode: 422),
                _ => Problem("Internal Error", statusCode: 500)
            };
        }

        return NoContent();
    }

    // DELETE: api/Authors/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id, CancellationToken ct)
    {
        var serviceResult = await _service.DeleteAuthorAsync(id, ct);

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
