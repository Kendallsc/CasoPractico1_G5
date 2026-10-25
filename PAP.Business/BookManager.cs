using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class BookManager
    {
        private readonly BookRepository _repository;

        public BookManager()
        {
            _repository = new BookRepository();
        }

        public IEnumerable<Book> GetAll()
        {
            return _repository.GetAll();
        }

        public Book GetById(int id)
        {
            return _repository.GetById(id);
        }

        // Se conserva el metodo original usado por LibraryController
        public IEnumerable<Book> GetAllBooksFromDataModel()
        {
            return _repository.GetAll();
        }

        public void Add(Book entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            if (string.IsNullOrWhiteSpace(entity.Name))
                entity.Name = entity.Title;
            SyncLegacyNames(entity);

            _repository.Add(entity);
        }

        public void Update(Book entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Book existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.Title = entity.Title;
            existing.Name = entity.Name;
            existing.Author = entity.Author;
            existing.Description = entity.Description;
            existing.PublicationDate = entity.PublicationDate;
            existing.Height = entity.Height;
            existing.Width = entity.Width;
            existing.Price = entity.Price;
            existing.IsBestSeller = entity.IsBestSeller;
            existing.IsInStock = entity.IsInStock;
            existing.PublisherId = entity.PublisherId;
            existing.CategoryId = entity.CategoryId;
            existing.GenreId = entity.GenreId;
            existing.ISBN = entity.ISBN;
            existing.Pages = entity.Pages;
            existing.Language = entity.Language;
            existing.CoverImageUrl = entity.CoverImageUrl;
            if (string.IsNullOrWhiteSpace(existing.Name))
                existing.Name = existing.Title;
            SyncLegacyNames(existing);
            existing.UpdatedAt = DateTime.UtcNow;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        // Pares Id / texto para llenar los dropdowns de las demas pantallas
        public IEnumerable<KeyValuePair<int, string>> GetOptions()
        {
            return _repository.Project(x => new { x.Id, x.Title })
                .Select(a => new KeyValuePair<int, string>(a.Id, a.Title))
                .OrderBy(o => o.Value)
                .ToList();
        }

        // Las columnas de texto Publisher / Category / Genre se mantienen sincronizadas
        // con los nombres de las nuevas llaves foraneas (la tabla Books ya existia).
        private static void SyncLegacyNames(Book book)
        {
            if (book.PublisherId.HasValue)
            {
                Publisher publisher = new PublisherRepository().GetById(book.PublisherId.Value);
                if (publisher != null) book.Publisher = publisher.Name;
            }
            if (book.CategoryId.HasValue)
            {
                Category category = new CategoryRepository().GetById(book.CategoryId.Value);
                if (category != null) book.Category = category.Name;
            }
            if (book.GenreId.HasValue)
            {
                Genre genre = new GenreRepository().GetById(book.GenreId.Value);
                if (genre != null) book.Genre = genre.Name;
            }
        }
    }
}
