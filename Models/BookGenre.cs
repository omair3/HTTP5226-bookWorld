using BookWorld.Models;

public class BookGenre
{
    public int BookId { get; set; }
    public Book? Book { get; set; } // Made nullable
    public int GenreId { get; set; }
    public Genre? Genre { get; set; } // Made nullable
}