using BookWorld.DTOs;
using BookWorld.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookWorld.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksMvcController : Controller
    {
        private readonly IBookWorldService _service;

        public BooksMvcController(IBookWorldService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<IActionResult> Index()
        {
            var books = await _service.GetAllBooksAsync();
            if (books == null)
            {
                Console.WriteLine("Warning: GetAllBooksAsync returned null.");
                books = new List<BookDTO>();
            }
            else
            {
                Console.WriteLine($"Fetched {books.Count} books.");
            }
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllGenres = (await _service.GetAllGenresAsync())?.Select(g => g.Name).ToList() ?? new List<string>();
            return View(new BookDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDTO bookDto)
        {
            if (ModelState.IsValid)
            {
                if (bookDto.Genres == null && !string.IsNullOrEmpty(Request.Form["Genres"]))
                {
                    bookDto.Genres = JsonSerializer.Deserialize<List<string>>(Request.Form["Genres"]) ?? new List<string>();
                }
                var createdBook = await _service.CreateBookAsync(bookDto);
                if (createdBook != null)
                {
                    TempData["Message"] = "Book created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.AllGenres = (await _service.GetAllGenresAsync())?.Select(g => g.Name).ToList() ?? new List<string>();
            return View(bookDto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _service.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            ViewBag.AllGenres = (await _service.GetAllGenresAsync())?.Select(g => g.Name).ToList() ?? new List<string>();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] BookDTO bookDto)
        {
            if (id != bookDto.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                if (bookDto.Genres == null && !string.IsNullOrEmpty(Request.Form["Genres"]))
                {
                    bookDto.Genres = JsonSerializer.Deserialize<List<string>>(Request.Form["Genres"]) ?? new List<string>();
                }
                Console.WriteLine($"Attempting to update book {id} with title: {bookDto.Title}");
                var updatedBook = await _service.UpdateBookAsync(id, bookDto);
                if (updatedBook != null)
                {
                    Console.WriteLine($"Successfully updated book {id}");
                    TempData["Message"] = "Book updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                Console.WriteLine($"Failed to update book {id}");
                ModelState.AddModelError(string.Empty, "Failed to update book.");
            }
            ViewBag.AllGenres = (await _service.GetAllGenresAsync())?.Select(g => g.Name).ToList() ?? new List<string>();
            return View(bookDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _service.GetBookByIdAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"Attempting to delete book {id}");
            var deleted = await _service.DeleteBookAsync(id);
            if (deleted)
            {
                Console.WriteLine($"Successfully deleted book {id}");
                TempData["Message"] = "Book deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine($"Failed to delete book {id}");
            ModelState.AddModelError(string.Empty, "Failed to delete book.");
            return View(await _service.GetBookByIdAsync(id));
        }
    }
}