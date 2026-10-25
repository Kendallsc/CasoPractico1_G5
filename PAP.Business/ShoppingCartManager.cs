using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class ShoppingCartManager
    {
        private readonly ShoppingCartRepository _repository;

        public ShoppingCartManager()
        {
            _repository = new ShoppingCartRepository();
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            return _repository.GetAll();
        }

        public ShoppingCart GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(ShoppingCart entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            _repository.Add(entity);
        }

        public void Update(ShoppingCart entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            ShoppingCart existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.CustomerId = entity.CustomerId;
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
            return _repository.Project(x => new { x.Id, x.CustomerId })
                .Select(a => new KeyValuePair<int, string>(a.Id, "Carrito #" + a.Id + " (cliente " + a.CustomerId + ")"))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
