using BookAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.ResponseDTOs;

public class BookGetResponseDto
{
    [Key]
    public int BookId { get; set; }
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public int Pages { get; set; }
    [DataType(DataType.Date)]
    public DateTime PublicationDate { get; set; }
    public IEnumerable<AuthorShortDto>? Authors { get; set; }
}
