namespace BookWorld.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public List<string> BookTitles { get; set; } = new List<string>();
    }
}