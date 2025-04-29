namespace BookAPI.DTOs.ResponseDTOs;

public class AuthorShortDto
{
    public int AuthorId { get; set; }
    public required string FullName { get; set; }
}
