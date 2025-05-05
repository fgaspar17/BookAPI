using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;

namespace BookAPI.Mappers;

public static class AuthorMapper
{
    public static Author? MapToEntity(this AuthorPostRequestDto authorRequest)
    {
        return new Author
        {
            FirstName = authorRequest.FirstName,
            LastName = authorRequest.LastName,
            Birthday = authorRequest.Birthday,
        };
    }

    public static Author? MapToEntity(this AuthorPutRequestDto authorRequest)
    {
        return new Author
        {
            AuthorId = authorRequest.AuthorId,
            FirstName = authorRequest.FirstName,
            LastName = authorRequest.LastName,
            Birthday = authorRequest.Birthday,
        };
    }

    public static AuthorShortDto? MapToShortDto(this Author author)
    {
        return new AuthorShortDto
        {
            AuthorId = author.AuthorId,
            FullName = author.FullName,
        };
    }

    public static AuthorResponseDTO? MapToDto(this Author author)
    {
        return new AuthorResponseDTO
        {
            AuthorId = author.AuthorId,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthday = author.Birthday,
            Books = author.AuthorBooks?.Select(authorBook => authorBook.Book).Select(book => book.MapToShortDto())
        };
    }
}
