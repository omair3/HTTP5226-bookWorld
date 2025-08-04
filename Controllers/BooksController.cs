using BookWorld.DTOs;
using BookWorld.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookWorldService _service;

        public BooksController(IBookWorldService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a list of all books with their authors and genres.
        /// </summary>
        /// <returns>A list of BookDTO objects.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> GetAllBooks()
        {
            var books = await _service.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>A BookDTO object or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _service.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        /// <summary>
        /// Creates a new book with associated author and genres.
        /// </summary>
        /// <param name="bookDto">The book data to create.</param>
        /// <returns>The created BookDTO.</returns>
        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBook(BookDTO bookDto)
        {
            var book = await _service.CreateBookAsync(bookDto);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="bookDto">The updated book data.</param>
        /// <returns>The updated BookDTO or 404 if not found.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBook(int id, BookDTO bookDto)
        {
            var book = await _service.UpdateBookAsync(id, bookDto);
            if (book == null) return NotFound();
            return Ok(book);
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>204 No Content or 404 if not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var success = await _service.DeleteBookAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Retrieves all books for a specific author.
        /// </summary>
        /// <param name="authorId">The ID of the author.</param>
        /// <returns>A list of BookDTO objects.</returns>
        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BookDTO>>> ListBooksForAuthor(int authorId)
        {
            var books = await _service.ListBooksForAuthorAsync(authorId);
            return Ok(books);
        }

        /// <summary>
        /// Retrieves all books associated with a specific genre.
        /// </summary>
        /// <param name="genreId">The ID of the genre.</param>
        /// <returns>A list of BookDTO objects or 404 if no books are found.</returns>
        [HttpGet("genre/{genreId}")]
        public async Task<ActionResult<List<BookDTO>>> GetBooksByGenre(int genreId)
        {
            var books = await _service.GetBooksByGenreAsync(genreId);
            if (books == null || !books.Any()) return NotFound();
            return Ok(books);
        }
    }
}