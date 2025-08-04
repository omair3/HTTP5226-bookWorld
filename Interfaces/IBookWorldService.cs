using BookWorld.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookWorld.Interfaces
{
    public interface IBookWorldService
    {
        // Book CRUD
        Task<List<BookDTO>> GetAllBooksAsync();
        Task<BookDTO> GetBookByIdAsync(int id);
        Task<BookDTO> CreateBookAsync(BookDTO bookDto);
        Task<BookDTO> UpdateBookAsync(int id, BookDTO bookDto);
        Task<bool> DeleteBookAsync(int id);

        // Author CRUD
        Task<List<AuthorDTO>> GetAllAuthorsAsync();
        Task<AuthorDTO> GetAuthorByIdAsync(int id);
        Task<AuthorDTO> CreateAuthorAsync(AuthorDTO authorDto);
        Task<AuthorDTO> UpdateAuthorAsync(int id, AuthorDTO authorDto);
        Task<bool> DeleteAuthorAsync(int id);

        // Genre CRUD
        Task<List<GenreDTO>> GetAllGenresAsync();
        Task<GenreDTO> GetGenreByIdAsync(int id);
        Task<GenreDTO> CreateGenreAsync(GenreDTO genreDto);
        Task<GenreDTO> UpdateGenreAsync(int id, GenreDTO genreDto);
        Task<bool> DeleteGenreAsync(int id);

        // Relational Methods
        Task<List<BookDTO>> ListBooksForAuthorAsync(int authorId);
        Task<List<GenreDTO>> ListGenresForBookAsync(int bookId);

        // New Method
        Task<List<BookDTO>> GetBooksByGenreAsync(int genreId);
    }
}