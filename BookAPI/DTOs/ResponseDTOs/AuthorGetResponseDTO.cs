using System.ComponentModel.DataAnnotations;
using BookAPI.Models;
using Newtonsoft.Json;

namespace BookAPI.DTOs.ResponseDTOs;

public class AuthorGetResponseDto
{
    [Key]
    public int AuthorId { get; set; }
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }
    [MaxLength(100)]
    public string? LastName { get; set; }
    [DataType(DataType.Date)]
    public DateTime Birthday { get; set; }
    public IEnumerable<BookShortDto>? Books { get; set; }
}
