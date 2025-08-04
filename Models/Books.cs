using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BookWorld.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}