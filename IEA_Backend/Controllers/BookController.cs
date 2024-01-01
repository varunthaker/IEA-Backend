using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IEA_Backend.models;
using IEA_Backend.Services.BookService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IEA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService BookService)
        {
            _bookService = BookService;
        }

        [HttpGet]
        public async  Task<ActionResult<List<Book>>> GetAllBooks()
        {
            return await _bookService.GetAllBooks();
           

        }
    }


}
