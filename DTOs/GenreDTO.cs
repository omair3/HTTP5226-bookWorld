namespace BookWorld.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> BookTitles { get; set; } = new List<string>();
    }
}