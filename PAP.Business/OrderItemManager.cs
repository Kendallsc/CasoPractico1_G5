using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class OrderItemManager
    {
        private readonly OrderItemRepository _repository;

        public OrderItemManager()
        {
            _repository = new OrderItemRepository();
        }

        public IEnumerable<OrderItem> GetAll()
        {
            return _repository.GetAll();
        }

        public OrderItem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(OrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(OrderItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            OrderItem existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.OrderId = entity.OrderId;
            existing.BookId = entity.BookId;
            existing.Quantity = entity.Quantity;
            existing.UnitPrice = entity.UnitPrice;
            existing.Discount = entity.Discount;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
