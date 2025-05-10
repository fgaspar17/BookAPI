using BookAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.DTOs.RequestDTOs
{
    public class BookPostRequestDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Pages { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
