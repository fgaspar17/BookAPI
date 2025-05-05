using BookAPI.DTOs.RequestDTOs;
using BookAPI.DTOs.ResponseDTOs;
using BookAPI.Models;

namespace BookAPI.Mappers;

public static class BookMapper
{
    public static Book? MapToEntity(this BookPostRequestDto bookRequest)
    {
        return new Book
        {
            Title = bookRequest.Title,
            Description = bookRequest.Description,
            Pages = bookRequest.Pages,
            PublicationDate = bookRequest.PublicationDate,
        };
    }

    public static Book? MapToEntity(this BookPutRequestDto bookRequest)
    {
        return new Book
        {
            BookId = bookRequest.BookId,
            Title = bookRequest.Title,
            Description = bookRequest.Description,
            Pages = bookRequest.Pages,
            PublicationDate = bookRequest.PublicationDate,
        };
    }

    public static BookShortDto? MapToShortDto(this Book book)
    {
        return new BookShortDto
        {
            BookId = book.BookId,
            Title = book.Title,
        };
    }

    public static BookResponseDTO? MapToDto(this Book book)
    {
        return new BookResponseDTO
        {
            BookId = book.BookId,
            Title = book.Title,
            Description = book.Description,
            Pages = book.Pages,
            PublicationDate = book.PublicationDate,
            Authors = book.BookAuthors?.Select(authorBook => authorBook.Author).Select(author => author!.MapToShortDto())
        };
    }
}
