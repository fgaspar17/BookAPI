using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.ResponseDTOs;

public class AuthorBookResponseDTO
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public int BookId { get; set; }
}
