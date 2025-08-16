public class GenreDTO
{
    public int Id { get; set; }
    public string? Name { get; set; } // Made nullable
    public List<string> BookTitles { get; set; } = new List<string>();
}