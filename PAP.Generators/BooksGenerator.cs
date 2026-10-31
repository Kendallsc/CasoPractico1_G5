using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class BooksGenerator : GeneratorBase<Book>
    {
        private const int Total = 100;

        public BooksGenerator()
            : base(new BookRepository())
        {
        }

        public override string EntityName
        {
            get { return "Books"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Publishers", "Categories", "Genres" }; }
        }

        protected override IEnumerable<Book> Build()
        {
            // Solo se usan Ids que existen en la base de datos, asi las llaves foraneas siempre son validas
            var publishers = new PublisherRepository().Project(x => new { x.Id, x.Name }).ToList();
            var categories = new CategoryRepository().Project(x => new { x.Id, x.Name }).ToList();
            var genres = new GenreRepository().Project(x => new { x.Id, x.Name }).ToList();

            DateTime now = DateTime.UtcNow;
            var books = new List<Book>();
            for (int i = 1; i <= Total; i++)
            {
                var publisher = Fake.Pick(publishers);
                var category = Fake.Pick(categories);
                var genre = Fake.Pick(genres);
                string title = Fake.BookTitle();

                books.Add(new Book
                {
                    Name = title,
                    Title = title,
                    Description = Fake.Paragraph(3),
                    PublicationDate = Fake.Date(new DateTime(1990, 1, 1), DateTime.Today),
                    Height = Fake.Money(18m, 28m),
                    Width = Fake.Money(12m, 21m),
                    Price = Fake.Money(7.5m, 65m),
                    IsBestSeller = Fake.Chance(15),
                    IsInStock = Fake.Chance(85),
                    PublisherId = publisher.Id,
                    CategoryId = category.Id,
                    GenreId = genre.Id,
                    // Columnas de texto heredadas de la tabla Books original
                    Publisher = publisher.Name,
                    Category = category.Name,
                    Genre = genre.Name,
                    ISBN = Fake.Isbn13(i),
                    Pages = Fake.Int(80, 900),
                    Language = Fake.Chance(70) ? "Espanol" : Fake.Pick(DataFaker.Languages),
                    CoverImageUrl = "https://picsum.photos/seed/book" + i + "/300/450",
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }
            return books;
        }
    }
}
