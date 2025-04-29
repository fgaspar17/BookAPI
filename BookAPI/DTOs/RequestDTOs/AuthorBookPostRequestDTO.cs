using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class AuthorBookPostRequestDto
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public int BookId { get; set; }
}
