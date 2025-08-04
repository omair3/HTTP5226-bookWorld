using BookWorld.DTOs;
using BookWorld.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IBookWorldService _service;

        public AuthorsController(IBookWorldService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a list of all authors with their books.
        /// </summary>
        /// <returns>A list of AuthorDTO objects.</returns>
        [HttpGet]
        public async Task<ActionResult<List<AuthorDTO>>> GetAllAuthors()
        {
            var authors = await _service.GetAllAuthorsAsync();
            return Ok(authors);
        }

        /// <summary>
        /// Retrieves an author by their ID.
        /// </summary>
        /// <param name="id">The ID of the author.</param>
        /// <returns>An AuthorDTO object or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var author = await _service.GetAuthorByIdAsync(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="authorDto">The author data to create.</param>
        /// <returns>The created AuthorDTO.</returns>
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor(AuthorDTO authorDto)
        {
            var author = await _service.CreateAuthorAsync(authorDto);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="id">The ID of the author to update.</param>
        /// <param name="authorDto">The updated author data.</param>
        /// <returns>The updated AuthorDTO or 404 if not found.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDTO>> UpdateAuthor(int id, AuthorDTO authorDto)
        {
            var author = await _service.UpdateAuthorAsync(id, authorDto);
            if (author == null) return NotFound();
            return Ok(author);
        }

        /// <summary>
        /// Deletes an author by their ID.
        /// </summary>
        /// <param name="id">The ID of the author to delete.</param>
        /// <returns>204 No Content or 404 if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var success = await _service.DeleteAuthorAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}