public class Genre
{
    public int Id { get; set; }
    public string? Name { get; set; } // Made nullable
    public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}