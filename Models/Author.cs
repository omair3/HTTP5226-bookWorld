namespace BookWorld.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Default to empty string
        public string Country { get; set; } = string.Empty; // Default to empty string
        public List<Book> Books { get; set; } = new List<Book>(); // Initialize
    }
}