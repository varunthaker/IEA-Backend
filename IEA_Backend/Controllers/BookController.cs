using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEA_Backend.models;
using IEA_Backend.Services.BookService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IEA_Backend.Controllers

{
    [EnableCors("AllowSpecificOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService BookService)
        {
            _bookService = BookService;
        }

        [HttpGet("allBooks")]
        public async Task<ActionResult<List<Book>>> GetAllBooks()

        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);

        }

        [HttpGet]
        public async  Task<ActionResult<List<string>>> GetAllBookTitles()

        {
            var bookTitles =  await _bookService.GetAllBookTitles();
            return Ok(bookTitles);

        }

      

        [HttpGet("{id}")]
        public async Task<ActionResult<List<string>>> GetTopWordsInBook(int id)
        {

            var top10Words = await _bookService.GetTopWordsInBook(id);

            if (top10Words == null)
            {
                return NotFound();
            }

            return Ok(top10Words);
        }

        [HttpGet("{id}/search/{searchString:minlength(3)}")]
        public async Task<ActionResult<List<string>>> SearchWordsInBook(int id, string searchString)
        {
            var matchedWords = await _bookService.SearchWordsInBook(id, searchString);

            if (matchedWords == null)
            {
                return NotFound(); 
            }

            return Ok(matchedWords);
        }

    }

}
