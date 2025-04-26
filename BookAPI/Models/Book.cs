using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models;

[Table("Books")]
public class Book
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
    public IEnumerable<AuthorBook>? BookAuthors { get; set; }
}
