namespace BookWorld.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
    }
}