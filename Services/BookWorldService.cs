using BookWorld.Data;
using BookWorld.DTOs;
using BookWorld.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWorld.Services
{
    public class BookWorldService : Interfaces.IBookWorldService
    {
        private readonly BookWorldContext _context;

        public BookWorldService(BookWorldContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<BookDTO>> GetAllBooksAsync()
        {
            var dbBooksCount = await _context.Books.CountAsync();
            Console.WriteLine($"Raw DB book count: {dbBooksCount}");

            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title ?? "Unknown Title",
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown Author",
                    Genres = b.BookGenres != null ? b.BookGenres.Select(bg => bg.Genre != null ? bg.Genre.Name : "Unknown Genre").ToList() : new List<string>(),
                    IsBestSeller = b.IsBestSeller
                })
                .ToListAsync();

            Console.WriteLine($"Mapped book count: {books.Count}");
            return books ?? new List<BookDTO>();
        }

        public async Task<BookDTO> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title ?? "Unknown Title",
                AuthorName = book.Author != null ? book.Author.Name : "Unknown Author",
                Genres = book.BookGenres != null ? book.BookGenres.Select(bg => bg.Genre != null ? bg.Genre.Name : "Unknown Genre").ToList() : new List<string>(),
                IsBestSeller = book.IsBestSeller
            };
        }

        public async Task<BookDTO> CreateBookAsync(BookDTO bookDto)
        {
            if (bookDto == null) throw new ArgumentNullException(nameof(bookDto));

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == bookDto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = bookDto.AuthorName ?? string.Empty, Country = string.Empty };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }

            var book = new Book
            {
                Title = bookDto.Title ?? string.Empty,
                AuthorId = author.Id,
                IsBestSeller = bookDto.IsBestSeller
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            if (bookDto.Genres != null)
            {
                foreach (var genreName in bookDto.Genres)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName ?? string.Empty };
                        _context.Genres.Add(genre);
                        await _context.SaveChangesAsync();
                    }

                    _context.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genre.Id });
                }
                await _context.SaveChangesAsync();
            }

            return await GetBookByIdAsync(book.Id);
        }

        public async Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto)
        {
            if (bookDto == null) throw new ArgumentNullException(nameof(bookDto));

            var book = await _context.Books
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == bookDto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = bookDto.AuthorName ?? string.Empty, Country = string.Empty };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }

            book.Title = bookDto.Title ?? string.Empty;
            book.AuthorId = author.Id;
            book.IsBestSeller = bookDto.IsBestSeller;

            _context.BookGenres.RemoveRange(book.BookGenres);
            if (bookDto.Genres != null)
            {
                foreach (var genreName in bookDto.Genres)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName ?? string.Empty };
                        _context.Genres.Add(genre);
                        await _context.SaveChangesAsync();
                    }

                    _context.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genre.Id });
                }
            }
            await _context.SaveChangesAsync();

            return await GetBookByIdAsync(id);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return false;

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<AuthorDTO>> GetAllAuthorsAsync()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    Name = a.Name ?? string.Empty,
                    Country = a.Country ?? string.Empty,
                    BookTitles = a.Books != null ? a.Books.Select(b => b.Title ?? "Unknown Title").ToList() : new List<string>()
                })
                .ToListAsync();

            return authors ?? new List<AuthorDTO>();
        }

        public async Task<AuthorDTO> GetAuthorByIdAsync(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null) return null;

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name ?? string.Empty,
                Country = author.Country ?? string.Empty,
                BookTitles = author.Books != null ? author.Books.Select(b => b.Title ?? "Unknown Title").ToList() : new List<string>()
            };
        }

        public async Task<AuthorDTO> CreateAuthorAsync(AuthorDTO authorDto)
        {
            if (authorDto == null) throw new ArgumentNullException(nameof(authorDto));

            var author = new Author
            {
                Name = authorDto.Name ?? string.Empty,
                Country = authorDto.Country ?? string.Empty
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return await GetAuthorByIdAsync(author.Id);
        }

        public async Task<AuthorDTO> UpdateAuthorAsync(int id, AuthorDTO authorDto)
        {
            if (authorDto == null) throw new ArgumentNullException(nameof(authorDto));

            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            author.Name = authorDto.Name ?? string.Empty;
            author.Country = authorDto.Country ?? string.Empty;
            await _context.SaveChangesAsync();
            return await GetAuthorByIdAsync(id);
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GenreDTO> CreateGenreAsync(GenreDTO genreDto)
        {
            if (genreDto == null) throw new ArgumentNullException(nameof(genreDto));

            var genre = new Genre { Name = genreDto.Name ?? string.Empty };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return await GetGenreByIdAsync(genre.Id);
        }

        public async Task<GenreDTO> UpdateGenreAsync(int id, GenreDTO genreDto)
        {
            if (genreDto == null) throw new ArgumentNullException(nameof(genreDto));

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return null;

            genre.Name = genreDto.Name ?? string.Empty;
            await _context.SaveChangesAsync();
            return await GetGenreByIdAsync(id);
        }

        public async Task<bool> DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<GenreDTO>> GetAllGenresAsync()
        {
            var genres = await _context.Genres
                .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .Select(g => new GenreDTO
                {
                    Id = g.Id,
                    Name = g.Name ?? string.Empty,
                    BookTitles = g.BookGenres != null ? g.BookGenres.Select(bg => bg.Book != null ? bg.Book.Title : "Unknown Title").ToList() : new List<string>()
                })
                .ToListAsync();

            return genres ?? new List<GenreDTO>();
        }

        public async Task<GenreDTO> GetGenreByIdAsync(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null) return null;

            return new GenreDTO
            {
                Id = genre.Id,
                Name = genre.Name ?? string.Empty,
                BookTitles = genre.BookGenres != null ? genre.BookGenres.Select(bg => bg.Book != null ? bg.Book.Title : "Unknown Title").ToList() : new List<string>()
            };
        }

        public async Task<List<BookDTO>> ListBooksForAuthorAsync(int authorId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title ?? "Unknown Title",
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown Author",
                    Genres = b.BookGenres != null ? b.BookGenres.Select(bg => bg.Genre != null ? bg.Genre.Name : "Unknown Genre").ToList() : new List<string>(),
                    IsBestSeller = b.IsBestSeller
                })
                .ToListAsync();

            return books ?? new List<BookDTO>();
        }

        public async Task<List<GenreDTO>> ListGenresForBookAsync(int bookId)
        {
            var genres = await _context.Genres
                .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .Where(g => g.BookGenres.Any(bg => bg.BookId == bookId))
                .Select(g => new GenreDTO
                {
                    Id = g.Id,
                    Name = g.Name ?? string.Empty,
                    BookTitles = g.BookGenres != null ? g.BookGenres.Select(bg => bg.Book != null ? bg.Book.Title : "Unknown Title").ToList() : new List<string>()
                })
                .ToListAsync();

            return genres ?? new List<GenreDTO>();
        }

        public async Task<List<BookDTO>> GetBooksByGenreAsync(int genreId)
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId))
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title ?? "Unknown Title",
                    AuthorName = b.Author != null ? b.Author.Name : "Unknown Author",
                    Genres = b.BookGenres != null ? b.BookGenres.Select(bg => bg.Genre != null ? bg.Genre.Name : "Unknown Genre").ToList() : new List<string>(),
                    IsBestSeller = b.IsBestSeller
                })
                .ToListAsync();

            return books ?? new List<BookDTO>();
        }
    }
}