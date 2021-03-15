using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModel;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class PublisherService
    {
        private AppDbContext _context;
        public PublisherService(AppDbContext context)
        {
            _context = context;
        }


        public Publisher AddPublisher(PublisherVM publisher)
        {

            if (StringStartsWithNumber(publisher.Name))
            {
                throw new PublisherNameException("Name starts with number", publisher.Name);
            }

            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;

        }

        public Publisher GetPublisherById(int publisherId) => _context.Publishers.FirstOrDefault(n => n.Id == publisherId);



        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            return _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(x => new BookAuthorVM()
                    {
                        BookName = x.Title,
                        BookAuthors = x.Book_Authors.Select(a => a.Author.FullName).ToList()

                    }).ToList()

                }).FirstOrDefault();


        }

        internal List<Publisher> GetAllPublisher(string sortBy, string searchString, int? pageNumer)
        {
            var allpublisher = _context.Publishers.OrderBy(n => n.Name).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allpublisher = _context.Publishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }

            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allpublisher = allpublisher.Where(n => n.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }

            int pageSize = 2;
            allpublisher = PaginatedList<Publisher>.Create(allpublisher.AsQueryable(), pageNumer ?? 1, pageSize);

            return allpublisher;
        }


        internal void DeletePublisherById(int publisherId)
        {
            var _publisher = _context.Publishers.FirstOrDefault(p => p.Id == publisherId);

            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"This publisher with id {publisherId} doesn't exists");
            }
        }


        private bool StringStartsWithNumber(string name) => Regex.IsMatch(name, @"^[\d]");
    }
}
