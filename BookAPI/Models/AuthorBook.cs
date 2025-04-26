using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models;

[Table("AuthorBooks")]
public class AuthorBook
{
    [Required]
    public int AuthorId { get; set; }
    [Required]
    public int BookId { get; set; }
    public Author? Author { get; set; }
    public Book? Book { get; set; }
}
