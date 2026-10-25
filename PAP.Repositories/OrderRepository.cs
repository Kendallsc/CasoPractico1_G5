using PAP.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PAP.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        // Carga las entidades relacionadas que muestra la vista Index (evita N+1 consultas)
        public override IEnumerable<Order> GetAll()
        {
            return _set.Include(x => x.Address).Include(x => x.Customer).ToList();
        }
    }
}
