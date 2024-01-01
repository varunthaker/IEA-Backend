using System;
namespace IEA_Backend.Services.BookService
{ 

	public interface IBookService
	{
        Task<List<Book>> GetAllBooks();
    }
}

