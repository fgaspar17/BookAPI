using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class AuthorBookPostRequestDto
{
    public int AuthorId { get; set; }
    public int BookId { get; set; }
}
