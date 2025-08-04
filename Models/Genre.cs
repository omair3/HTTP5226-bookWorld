using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BookWorld.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
    }
}