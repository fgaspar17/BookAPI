using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;

namespace BookAPI.Mappers;

public static class AuthorBookMapper
{
    public static AuthorBook? MapToEntity(this AuthorBookPostRequestDto authorBookRequest)
    {
        return new AuthorBook
        {
            AuthorId = authorBookRequest.AuthorId,
            BookId = authorBookRequest.BookId,
        };
    }

    public static AuthorBookResponseDTO MapToDto(this AuthorBook authorBook)
    {
        return new AuthorBookResponseDTO
        {
            AuthorId = authorBook.AuthorId,
            BookId = authorBook.BookId,
        };
    }
}
