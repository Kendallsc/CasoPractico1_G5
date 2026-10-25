using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class PaymentManager
    {
        private readonly PaymentRepository _repository;

        public PaymentManager()
        {
            _repository = new PaymentRepository();
        }

        public IEnumerable<Payment> GetAll()
        {
            return _repository.GetAll();
        }

        public Payment GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Payment entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            if (entity.PaymentDate == default(DateTime))
                entity.PaymentDate = now;

            _repository.Add(entity);
        }

        public void Update(Payment entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Payment existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.OrderId = entity.OrderId;
            existing.PaymentMethod = entity.PaymentMethod;
            existing.Amount = entity.Amount;
            existing.PaymentDate = entity.PaymentDate;
            existing.Status = entity.Status;
            existing.TransactionRef = entity.TransactionRef;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
