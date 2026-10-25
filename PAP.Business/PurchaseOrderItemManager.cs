using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class PurchaseOrderItemManager
    {
        private readonly PurchaseOrderItemRepository _repository;

        public PurchaseOrderItemManager()
        {
            _repository = new PurchaseOrderItemRepository();
        }

        public IEnumerable<PurchaseOrderItem> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseOrderItem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(PurchaseOrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(PurchaseOrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            PurchaseOrderItem existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.PurchaseOrderId = entity.PurchaseOrderId;
            existing.BookId = entity.BookId;
            existing.Quantity = entity.Quantity;
            existing.UnitCost = entity.UnitCost;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
