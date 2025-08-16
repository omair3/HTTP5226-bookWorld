namespace BookWorld.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; } // Nullable for binding
        public string? AuthorName { get; set; } // Nullable for binding
        public List<string> Genres { get; set; } = new List<string>(); // Initialize
        public bool IsBestSeller { get; set; } = false; // Default to false
    }
}