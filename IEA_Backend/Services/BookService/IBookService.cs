using System;
namespace IEA_Backend.Services.BookService
{ 

	public interface IBookService
	{
        Task ProcessBooksAsync();
        Task<List<Book>> GetAllBooks();
        Task<List<string>> GetAllBookTitles();
        Task<List<string>> GetTopWordsInBook(int id);
        Task<List<string>> SearchWordsInBook(int id, string searchString);

    }
}

