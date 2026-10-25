using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class CartItemManager
    {
        private readonly CartItemRepository _repository;

        public CartItemManager()
        {
            _repository = new CartItemRepository();
        }

        public IEnumerable<CartItem> GetAll()
        {
            return _repository.GetAll();
        }

        public CartItem GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(CartItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.AddedAt = now;

            _repository.Add(entity);
        }

        public void Update(CartItem entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            CartItem existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.CartId = entity.CartId;
            existing.BookId = entity.BookId;
            existing.Quantity = entity.Quantity;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
