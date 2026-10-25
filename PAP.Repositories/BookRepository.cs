using PAP.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PAP.Repositories
{
    public class BookRepository : RepositoryBase<Book>
    {
        // Carga las entidades relacionadas que muestra la vista Index (evita N+1 consultas)
        public override IEnumerable<Book> GetAll()
        {
            return _set.Include(x => x.Category1).Include(x => x.Genre1).Include(x => x.Publisher1).ToList();
        }
    }
}
