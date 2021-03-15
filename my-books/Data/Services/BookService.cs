using my_books.Data.Models;
using my_books.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class BookService
    {
        private AppDbContext _context;
        public BookService(AppDbContext context)
        {
            _context = context;
        }

        public void AddBookWithAuthors(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                //Author = book.Author,
                CovertUrl = book.CovertUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId
            };
            _context.Books.Add(_book);
            _context.SaveChanges();

            foreach (var id in book.AuthorIds)
            {
                var _book_Author = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };

                _context.Book_Authors.Add(_book_Author);
                _context.SaveChanges();
            }
        
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();

        //public Book GetBookById(int bookId) => _context.Books.FirstOrDefault(book => book.Id == bookId);

        public BookWithAuthorsVM GetBookWithAuthorsById(int bookId) {

            var _bookWithAuthor = _context.Books.Where(book => book.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                //Author = book.Author,
                CovertUrl = book.CovertUrl,
                
                PublisherName = book.Publisher.Name,
                AuthorNames = book.Book_Authors.Select( n=> n.Author.FullName).ToList()
            }).FirstOrDefault();
            return _bookWithAuthor;
        }


        public Book UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(book => book.Id == bookId);

            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                //_book.Author = book.Author;
                _book.CovertUrl = book.CovertUrl;
                _book.DateAdded = DateTime.Now;

                _context.SaveChanges();
            }
            return _book;
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(book => book.Id == bookId);

            if (_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }


    }
}
