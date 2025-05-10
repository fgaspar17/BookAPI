using BookAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class BookPutRequestDto
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int Pages { get; set; }
    [DataType(DataType.Date)]
    public DateTime PublicationDate { get; set; }
}
