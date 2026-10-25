using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class CustomerManager
    {
        private readonly CustomerRepository _repository;

        public CustomerManager()
        {
            _repository = new CustomerRepository();
        }

        public IEnumerable<Customer> GetAll()
        {
            return _repository.GetAll();
        }

        public Customer GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Customer entity, string password)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contrasena es obligatoria.");
            entity.PasswordHash = PasswordHasher.Hash(password);

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;

            _repository.Add(entity);
        }

        public void Update(Customer entity, string password)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Customer existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.FirstName = entity.FirstName;
            existing.LastName = entity.LastName;
            existing.Email = entity.Email;
            existing.Phone = entity.Phone;
            existing.BirthDate = entity.BirthDate;
            existing.IsActive = entity.IsActive;

            // Si no se escribe una contrasena nueva se conserva la actual
            if (!string.IsNullOrWhiteSpace(password))
                existing.PasswordHash = PasswordHasher.Hash(password);
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
            return _repository.Project(x => new { x.Id, x.FirstName, x.LastName, x.Email })
                .Select(a => new KeyValuePair<int, string>(a.Id, a.FirstName + " " + a.LastName + " (" + a.Email + ")"))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
