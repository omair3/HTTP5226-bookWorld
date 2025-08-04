using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BookWorld.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}