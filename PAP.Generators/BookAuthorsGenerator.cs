using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class BookAuthorsGenerator : GeneratorBase<BookAuthor>
    {
        public BookAuthorsGenerator()
            : base(new BookAuthorRepository())
        {
        }

        public override string EntityName
        {
            get { return "BookAuthors"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Books", "Authors" }; }
        }

        protected override IEnumerable<BookAuthor> Build()
        {
            List<int> bookIds = new BookRepository().Project(x => x.Id).ToList();
            List<int> authorIds = new AuthorRepository().Project(x => x.Id).ToList();

            // Cada libro recibe de 1 a 2 autores distintos: la llave (BookId, AuthorId) nunca se repite
            var items = new List<BookAuthor>();
            foreach (int bookId in bookIds)
            {
                int howMany = Fake.Chance(75) ? 1 : 2;
                foreach (int authorId in Fake.PickMany(authorIds, howMany))
                    items.Add(new BookAuthor { BookId = bookId, AuthorId = authorId });
            }
            return items;
        }

        // Rellena la columna de texto Books.Author con los autores asignados, para que ambas coincidan
        protected override void AfterGenerate(List<BookAuthor> items)
        {
            Dictionary<int, string> authorNames = new AuthorRepository()
                .Project(x => new { x.Id, x.FirstName, x.LastName })
                .ToDictionary(a => a.Id, a => a.FirstName + " " + a.LastName);

            var namesByBook = items
                .GroupBy(x => x.BookId)
                .ToDictionary(g => g.Key, g => string.Join(", ", g.Select(x => authorNames[x.AuthorId])));

            var bookRepository = new BookRepository();
            foreach (Book book in bookRepository.GetAll())
            {
                string names;
                if (namesByBook.TryGetValue(book.Id, out names) && string.IsNullOrEmpty(book.Author))
                    book.Author = names.Length > 200 ? names.Substring(0, 200) : names;
            }
            bookRepository.Save();
        }
    }
}
