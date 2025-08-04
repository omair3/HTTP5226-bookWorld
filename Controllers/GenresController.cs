using BookWorld.DTOs;
using BookWorld.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IBookWorldService _service;

        public GenresController(IBookWorldService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a list of all genres with their associated books.
        /// </summary>
        /// <returns>A list of GenreDTO objects.</returns>
        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> GetAllGenres()
        {
            var genres = await _service.GetAllGenresAsync();
            return Ok(genres);
        }

        /// <summary>
        /// Retrieves a genre by its ID.
        /// </summary>
        /// <param name="id">The ID of the genre.</param>
        /// <returns>A GenreDTO object or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            var genre = await _service.GetGenreByIdAsync(id);
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        /// <summary>
        /// Creates a new genre.
        /// </summary>
        /// <param name="genreDto">The genre data to create.</param>
        /// <returns>The created GenreDTO.</returns>
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> CreateGenre(GenreDTO genreDto)
        {
            var genre = await _service.CreateGenreAsync(genreDto);
            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        /// <summary>
        /// Updates an existing genre.
        /// </summary>
        /// <param name="id">The ID of the genre to update.</param>
        /// <param name="genreDto">The updated genre data.</param>
        /// <returns>The updated GenreDTO or 404 if not found.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GenreDTO>> UpdateGenre(int id, GenreDTO genreDto)
        {
            var genre = await _service.UpdateGenreAsync(id, genreDto);
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        /// <summary>
        /// Deletes a genre by its ID.
        /// </summary>
        /// <param name="id">The ID of the genre to delete.</param>
        /// <returns>204 No Content or 404 if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var success = await _service.DeleteGenreAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retrieves all genres for a specific book.
        /// </summary>
        /// <param name="bookId">The ID of the book.</param>
        /// <returns>A list of GenreDTO objects.</returns>
        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<List<GenreDTO>>> ListGenresForBook(int bookId)
        {
            var genres = await _service.ListGenresForBookAsync(bookId);
            return Ok(genres);
        }
    }
}