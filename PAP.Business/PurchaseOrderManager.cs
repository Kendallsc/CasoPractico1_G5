using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class PurchaseOrderManager
    {
        private readonly PurchaseOrderRepository _repository;

        public PurchaseOrderManager()
        {
            _repository = new PurchaseOrderRepository();
        }

        public IEnumerable<PurchaseOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseOrder GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(PurchaseOrder entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            if (entity.OrderDate == default(DateTime))
                entity.OrderDate = now;

            _repository.Add(entity);
        }

        public void Update(PurchaseOrder entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            PurchaseOrder existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.SupplierId = entity.SupplierId;
            existing.OrderDate = entity.OrderDate;
            existing.ExpectedDate = entity.ExpectedDate;
            existing.Status = entity.Status;
            existing.TotalCost = entity.TotalCost;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        // Pares Id / texto para llenar los dropdowns de las demas pantallas
        public IEnumerable<KeyValuePair<int, string>> GetOptions()
        {
            return _repository.Project(x => new { x.Id, x.OrderDate })
                .Select(a => new KeyValuePair<int, string>(a.Id, "OC #" + a.Id + " - " + a.OrderDate.ToString("yyyy-MM-dd")))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
