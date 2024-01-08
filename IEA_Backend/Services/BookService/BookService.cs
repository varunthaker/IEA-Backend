using System;
using IEA_Backend.models;
using Microsoft.AspNetCore.Mvc;
using IEA_Backend.Services.BookService;
using System.Text.RegularExpressions;
using System.Text;
using IEA_Backend.Models;

namespace IEA_Backend.Services.BookService
{
    public class BookService : IBookService

    {

        private readonly string booksSourceFolderPath = "wwwroot/Books";
        private readonly string booksUTF8FolderPath = "wwwroot/UTF8Books";


        // books as a UTF8 Text file
        public async Task ProcessBooksAsync()
        {
            if (!Directory.Exists(booksSourceFolderPath))
            {
                return;
            }

            // Get all files with a .txt extension from the source folder
            string[] bookFiles = Directory.GetFiles(booksSourceFolderPath, "*.txt");

            foreach (string sourceFile in bookFiles)
            {
                // Read content from the source file
                string content = await File.ReadAllTextAsync(sourceFile, Encoding.UTF8);

                // Extract title from the file name
                string fileName = Path.GetFileNameWithoutExtension(sourceFile);

                // Save the book in UTF-8 format to the destination folder
                string destinationFilePath = Path.Combine(booksUTF8FolderPath, $"{fileName}.txt");
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(content);
                await File.WriteAllBytesAsync(destinationFilePath, utf8Bytes);
            }
        }

        // Get All Book objects list

        public async Task<List<Book>> GetAllBooks() {

            List<Book> books = new List<Book>();

            foreach (string filePath in Directory.GetFiles(booksUTF8FolderPath, "*.txt"))

            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                var parts = fileName.Split('-');


                if (parts.Length == 2 && int.TryParse(parts[0].Trim(), out int bookId))
                {
                    string title = parts[1].Trim();
                    string content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);

                    var book = new Book
                    {
                        Id = bookId,
                        Title = title,
                        Content = content
                    };

                    books.Add(book);
                }
            }

            return books;

        }


        // Get Book titles
        public async Task<List<BookInfo>> GetAllBookTitles()
        {

            List<Book> books = await GetAllBooks();

            List<BookInfo> bookInfoList = books.Select(book => new BookInfo
            {
                Id = book.Id,
                Title = book.Title
            }).ToList();


            return bookInfoList;

        }


        // Get Top10 words for a specific book
        public async Task<List<string>> GetTopWordsInBook(int id)
        {

            List<Book> books = await GetAllBooks();
            Book book = books.Find(book => book.Id == id);

            if (book == null)
            {
                return null; 
            }

            // Tokenize the content into words
            string[] words = TokenizeContent(book.Content);

            // Filter out short words (less than 5 characters)
            var filteredWords = words.Where(word => word.Length >= 5);

            // Group words by their count
            var wordGroups = filteredWords.GroupBy(word => word.ToLower());

            // Order groups by count in descending order
            var sortedWordGroups = wordGroups.OrderByDescending(group => group.Count());

            // Take top 10 groups
            var top10WordGroups = sortedWordGroups.Take(10);

            // Extract words from groups
            var top10Words = top10WordGroups.Select(group => Char.ToUpper(group.Key[0]) + group.Key.Substring(1)).ToList();

            return top10Words;
        }

        // Helper method to tokenize content into words
        private string[] TokenizeContent(string content)
        {
            // Use regular expression to split content into words
            return Regex.Split(content, @"\W+")
                        .Where(word => !string.IsNullOrWhiteSpace(word))
                        .ToArray();
        }

        // Get list of Words for the specific book and keyword
        public async Task<List<string>> SearchWordsInBook(int id, string searchString)
        {
            List<Book> books = await GetAllBooks();
            Book book = books.Find(book => book.Id == id);

            if (book == null)
            {
                return null;
            }

            string[] words = TokenizeContent(book.Content);

            // Perform case-insensitive search for words starting with the specified string
            var matchedWords = words.Where(word => word.StartsWith(searchString, StringComparison.OrdinalIgnoreCase));

            var matchedWordGroups = matchedWords.GroupBy(word => word.ToLower());

            // Order groups by count in descending order
            var sortedWordGroups = matchedWordGroups.OrderByDescending(group => group.Count());

            // Take top 10 groups
            var top10WordGroups = sortedWordGroups.Take(10);

            // Extract words from groups
            var matchedTop10Words = top10WordGroups.Select(group => Char.ToUpper(group.Key[0]) + group.Key.Substring(1)).ToList();

            return matchedTop10Words;
        }

    }

}