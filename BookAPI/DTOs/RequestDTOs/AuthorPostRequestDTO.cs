using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class AuthorPostRequestDto
{
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Birthday { get; set; }
}
