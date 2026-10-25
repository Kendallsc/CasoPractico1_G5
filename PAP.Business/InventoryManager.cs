using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class InventoryManager
    {
        private readonly InventoryRepository _repository;

        public InventoryManager()
        {
            _repository = new InventoryRepository();
        }

        public IEnumerable<Inventory> GetAll()
        {
            return _repository.GetAll();
        }

        public Inventory GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Inventory entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(Inventory entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Inventory existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.BookId = entity.BookId;
            existing.QuantityInStock = entity.QuantityInStock;
            existing.ReorderLevel = entity.ReorderLevel;
            existing.LastRestockedAt = entity.LastRestockedAt;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
