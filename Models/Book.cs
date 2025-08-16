using BookWorld.Models;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; } // Made nullable
    public int AuthorId { get; set; }
    public Author? Author { get; set; } // Made nullable
    public bool IsBestSeller { get; set; }
    public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
}