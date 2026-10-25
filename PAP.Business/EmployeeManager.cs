using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class EmployeeManager
    {
        private readonly EmployeeRepository _repository;

        public EmployeeManager()
        {
            _repository = new EmployeeRepository();
        }

        public IEnumerable<Employee> GetAll()
        {
            return _repository.GetAll();
        }

        public Employee GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Employee entity, string password)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contrasena es obligatoria.");
            entity.PasswordHash = PasswordHasher.Hash(password);

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;

            _repository.Add(entity);
        }

        public void Update(Employee entity, string password)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Employee existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.RoleId = entity.RoleId;
            existing.FirstName = entity.FirstName;
            existing.LastName = entity.LastName;
            existing.Email = entity.Email;
            existing.IsActive = entity.IsActive;

            // Si no se escribe una contrasena nueva se conserva la actual
            if (!string.IsNullOrWhiteSpace(password))
                existing.PasswordHash = PasswordHasher.Hash(password);

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
