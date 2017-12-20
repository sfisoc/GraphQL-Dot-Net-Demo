using System.Linq;
using System.Collections.Generic;

using GenFu;
using System;

using Ql.Models;



namespace Ql.Data
{
    public class BookRepository : IBookRepository
    {
        private static IEnumerable<Book> _books = new List<Book>();
        private static IEnumerable<Author> _authors = new List<Author>();
        private static IEnumerable<Publisher> _publisher = new List<Publisher>();

        public BookRepository()
        {
            GenFu.GenFu.Configure<Author>()
                .Fill(_ => _.Name)
                .AsLastName()
                .Fill(_=>_.Birthdate)
                .AsPastDate();

            if(_authors.Count()<1)
            {
                _authors = A.ListOf<Author>(40);
            }
           

            GenFu.GenFu.Configure<Publisher>()
                .Fill(_ => _.Name)
                .AsMusicArtistName();

            if (_publisher.Count() < 1)
            {
                _publisher = A.ListOf<Publisher>(10);
            }
           

            GenFu.GenFu.Configure<Book>()
               .Fill(p => p.Isbn)
               .AsISBN()
               .Fill(p => p.Name)
               .AsLoremIpsumWords(5)
               .Fill(p => p.Author)
               .WithRandom(_authors)
               .Fill(_ => _.Publisher)
               .WithRandom(_publisher);

            if (_books.Count() < 1)
            {
                _books = A.ListOf<Book>(100);
            }

        
        }

        public IEnumerable<Author> AllAuthors()
        {
            return _authors;
        }

        public IEnumerable<Book> AllBooks()
        {
            return _books;
        }

        public IEnumerable<Publisher> AllPublishers()
        {
            return _publisher;
        }

        public IEnumerable<Book> AuthorBooks(int id)
        {
            
            return _books.Where(a => a.Author.Id == id);
        }

        public Author AuthorById(int id)
        {
            return _authors.First(_ => _.Id == id);
        }

        public Book BookByIsbn(string isbn)
        {
            return _books.First(_ => _.Isbn == isbn);
        }

        public IEnumerable<Book> PublishedBooks(int id)
        {
            return _books.Where(a=>a.Publisher.Id==id);
        }

        public Publisher PublisherById(int id)
        {
            return _publisher.First(_ => _.Id == id);
        }
    }

    public static class StringFillerExtensions
    {
        
        public static GenFuConfigurator<T> AsISBN<T>(this GenFuStringConfigurator<T> configurator) where T : new()
        {
            var filler = new CustomFiller<string>(configurator.PropertyInfo.Name, typeof(T), () =>
            {
                return MakeIsbn();
            });
            configurator.Maggie.RegisterFiller(filler);
            return configurator;
        }
        public static string MakeIsbn()
        {
            // 978-1-933988-27-6
            var a = A.Random.Next(100, 999);
            var b = A.Random.Next(1, 9);
            var c = A.Random.Next(100000, 999999);
            var d = A.Random.Next(10, 99);
            var e = A.Random.Next(1, 9);

            return $"{a}-{b}-{c}-{d}-{e}";
        }
    }
}
