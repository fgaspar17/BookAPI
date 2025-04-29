using BookAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class AuthorPutRequestDto
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
}
