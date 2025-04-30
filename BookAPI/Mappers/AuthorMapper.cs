using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;

namespace BookAPI.Mappers;

public static class AuthorMapper
{
    public static Author? MapToEntity(this AuthorPostRequestDto authorRequest)
    {
        if (authorRequest == null) return null;

        return new Author
        {
            FirstName = authorRequest.FirstName,
            LastName = authorRequest.LastName,
            Birthday = authorRequest.Birthday,
        };
    }

    public static Author? MapToEntity(this AuthorPutRequestDto authorRequest)
    {
        if (authorRequest == null) return null;

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
        if (author == null) return null;

        return new AuthorShortDto
        {
            AuthorId = author.AuthorId,
            FullName = author.FullName,
        };
    }

    public static AuthorGetResponseDto? MapToDto(this Author author)
    {
        if (author == null) return null;

        return new AuthorGetResponseDto
        {
            AuthorId = author.AuthorId,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Birthday = author.Birthday,
            Books = author.AuthorBooks?.Select(authorBook => authorBook.Book).Select(book => book.MapToShortDto())
        };
    }
}
