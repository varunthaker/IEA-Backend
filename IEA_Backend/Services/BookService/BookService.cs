using System;
using IEA_Backend.models;
using Microsoft.AspNetCore.Mvc;
using IEA_Backend.Services.BookService;

namespace IEA_Backend.Services.BookService
{
    public class BookService : IBookService

    {
        private static List<Book> books = new List<Book> {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" },
                new Book { Id = 3, Title = "Book 3" },

            };

        public async Task<List<Book>> GetAllBooks()
        {
            return books;
        }
    }

}