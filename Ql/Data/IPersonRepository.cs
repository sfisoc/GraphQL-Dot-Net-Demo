﻿using Ql.Models;
using System.Collections.Generic;

namespace Ql.Data
{
    public interface IBookRepository
    {
        Book BookByIsbn(string isbn);
        IEnumerable<Book> AllBooks();

        Author AuthorById(int id);
        IEnumerable<Author> AllAuthors();

        Publisher PublisherById(int id);
        IEnumerable<Publisher> AllPublishers();

        IEnumerable<Book> AuthorBooks(int id);
        IEnumerable<Book> PublishedBooks(int id);
    }
}