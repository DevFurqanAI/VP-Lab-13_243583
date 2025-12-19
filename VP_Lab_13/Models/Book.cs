using System.ComponentModel.DataAnnotations;

namespace VP_Lab_13.Models
{
    public class Book
    {
        [Required]
        [Key]
        public int BookId { get; set; }

        [Required]
        public string? BookTitle { get; set; }
    }
}
