using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Business
{
    public class BookAuthorManager
    {
        private readonly BookAuthorRepository _repository;

        public BookAuthorManager()
        {
            _repository = new BookAuthorRepository();
        }

        public IEnumerable<BookAuthor> GetAll()
        {
            return _repository.GetAll();
        }

        public BookAuthor GetByKeys(int bookId, int authorId)
        {
            return _repository.GetByKeys(bookId, authorId);
        }

        public void Add(BookAuthor entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        // La llave es (BookId, AuthorId): "editar" significa cambiar la pareja.
        // Primero se agrega la nueva y luego se borra la anterior para no perder datos si algo falla.
        public void Replace(int oldBookId, int oldAuthorId, int bookId, int authorId)
        {
            if (oldBookId == bookId && oldAuthorId == authorId)
                return;

            _repository.Add(new BookAuthor { BookId = bookId, AuthorId = authorId });
            _repository.Delete(oldBookId, oldAuthorId);
        }

        public void Delete(int bookId, int authorId)
        {
            _repository.Delete(bookId, authorId);
        }
    }
}
