using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class OrderManager
    {
        private readonly OrderRepository _repository;

        public OrderManager()
        {
            _repository = new OrderRepository();
        }

        public IEnumerable<Order> GetAll()
        {
            return _repository.GetAll();
        }

        public Order GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Order entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.UpdatedAt = now;
            if (entity.OrderDate == default(DateTime))
                entity.OrderDate = now;

            entity.Total = entity.Subtotal + entity.TaxAmount + entity.ShippingCost;

            _repository.Add(entity);
        }

        public void Update(Order entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Order existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.CustomerId = entity.CustomerId;
            existing.ShippingAddressId = entity.ShippingAddressId;
            existing.OrderDate = entity.OrderDate;
            existing.Status = entity.Status;
            existing.Subtotal = entity.Subtotal;
            existing.TaxAmount = entity.TaxAmount;
            existing.ShippingCost = entity.ShippingCost;
            existing.Notes = entity.Notes;
            existing.Total = existing.Subtotal + existing.TaxAmount + existing.ShippingCost;
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
            return _repository.Project(x => new { x.Id, x.OrderDate })
                .Select(a => new KeyValuePair<int, string>(a.Id, "#" + a.Id + " - " + a.OrderDate.ToString("yyyy-MM-dd")))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
