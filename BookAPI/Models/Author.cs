using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models;

[Table("Authors")]
public class Author
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
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
    public IEnumerable<AuthorBook>? AuthorBooks { get; set; } = new List<AuthorBook>();
}
