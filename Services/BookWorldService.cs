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
            _context = context;
        }

        // Book CRUD
        public async Task<List<BookDTO>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.Name,
                    Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
                })
                .ToListAsync();
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
                Title = book.Title,
                AuthorName = book.Author.Name,
                Genres = book.BookGenres.Select(bg => bg.Genre.Name).ToList()
            };
        }

        public async Task<BookDTO> CreateBookAsync(BookDTO bookDto)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == bookDto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = bookDto.AuthorName, Country = "" };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }

            var book = new Book
            {
                Title = bookDto.Title,
                AuthorId = author.Id
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            foreach (var genreName in bookDto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Name = genreName };
                    _context.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                }

                _context.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genre.Id });
            }

            await _context.SaveChangesAsync();
            return await GetBookByIdAsync(book.Id);
        }

        public async Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto)
        {
            var book = await _context.Books
                .Include(b => b.BookGenres)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return null;

            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == bookDto.AuthorName);
            if (author == null)
            {
                author = new Author { Name = bookDto.AuthorName, Country = "" };
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();
            }

            book.Title = bookDto.Title;
            book.AuthorId = author.Id;

            _context.BookGenres.RemoveRange(book.BookGenres);
            foreach (var genreName in bookDto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
                if (genre == null)
                {
                    genre = new Genre { Name = genreName };
                    _context.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                }

                _context.BookGenres.Add(new BookGenre { BookId = book.Id, GenreId = genre.Id });
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

        // Author CRUD
        public async Task<List<AuthorDTO>> GetAllAuthorsAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Country = a.Country,
                    BookTitles = a.Books.Select(b => b.Title).ToList()
                })
                .ToListAsync();
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
                Name = author.Name,
                Country = author.Country,
                BookTitles = author.Books.Select(b => b.Title).ToList()
            };
        }

        public async Task<AuthorDTO> CreateAuthorAsync(AuthorDTO authorDto)
        {
            var author = new Author
            {
                Name = authorDto.Name,
                Country = authorDto.Country
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return await GetAuthorByIdAsync(author.Id);
        }

        public async Task<AuthorDTO> UpdateAuthorAsync(int id, AuthorDTO authorDto)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            author.Name = authorDto.Name;
            author.Country = authorDto.Country;
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

        // Genre CRUD
        public async Task<List<GenreDTO>> GetAllGenresAsync()
        {
            return await _context.Genres
                .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .Select(g => new GenreDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    BookTitles = g.BookGenres.Select(bg => bg.Book.Title).ToList()
                })
                .ToListAsync();
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
                Name = genre.Name,
                BookTitles = genre.BookGenres.Select(bg => bg.Book.Title).ToList()
            };
        }

        public async Task<GenreDTO> CreateGenreAsync(GenreDTO genreDto)
        {
            var genre = new Genre { Name = genreDto.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return await GetGenreByIdAsync(genre.Id);
        }

        public async Task<GenreDTO> UpdateGenreAsync(int id, GenreDTO genreDto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return null;

            genre.Name = genreDto.Name;
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

        // Relational Methods
        public async Task<List<BookDTO>> ListBooksForAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.Name,
                    Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<GenreDTO>> ListGenresForBookAsync(int bookId)
        {
            return await _context.Genres
                .Include(g => g.BookGenres).ThenInclude(bg => bg.Book)
                .Where(g => g.BookGenres.Any(bg => bg.BookId == bookId))
                .Select(g => new GenreDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    BookTitles = g.BookGenres.Select(bg => bg.Book.Title).ToList()
                })
                .ToListAsync();
        }

        // New Method
        public async Task<List<BookDTO>> GetBooksByGenreAsync(int genreId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookGenres).ThenInclude(bg => bg.Genre)
                .Where(b => b.BookGenres.Any(bg => bg.GenreId == genreId))
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.Name,
                    Genres = b.BookGenres.Select(bg => bg.Genre.Name).ToList()
                })
                .ToListAsync();
        }
    }
}