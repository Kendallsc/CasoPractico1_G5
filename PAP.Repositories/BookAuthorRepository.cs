using PAP.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PAP.Repositories
{
    public class BookAuthorRepository : RepositoryBase<BookAuthor>
    {
        // Carga las entidades relacionadas que muestra la vista Index (evita N+1 consultas)
        public override IEnumerable<BookAuthor> GetAll()
        {
            return _set.Include(x => x.Author).Include(x => x.Book).ToList();
        }

        // BookAuthors usa llave compuesta (BookId, AuthorId): no existe un Id simple.
        public override BookAuthor GetById(int id)
        {
            throw new NotSupportedException("BookAuthors usa llave compuesta. Use GetByKeys(bookId, authorId).");
        }

        public override void Delete(int id)
        {
            throw new NotSupportedException("BookAuthors usa llave compuesta. Use Delete(bookId, authorId).");
        }

        public BookAuthor GetByKeys(int bookId, int authorId)
        {
            return _set.Find(bookId, authorId);
        }

        public void Delete(int bookId, int authorId)
        {
            BookAuthor entity = GetByKeys(bookId, authorId);
            if (entity != null)
            {
                _set.Remove(entity);
                Save();
            }
        }
    }
}
