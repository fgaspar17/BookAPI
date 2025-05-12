using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs;

public class LoginDto
{
    [Required]
    [MaxLength(255)]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}
