using BookAPI.DTOs.RequestDTOs;
using BookAPI.Models;

namespace BookAPI.Mappers;

public static class AuthorBookMapper
{
    public static AuthorBook? MapToEntity(this AuthorBookPostRequestDto authorBookRequest)
    {
        if (authorBookRequest == null) return null;

        return new AuthorBook
        {
            AuthorId = authorBookRequest.AuthorId,
            BookId = authorBookRequest.BookId,
        };
    }
}
