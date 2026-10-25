using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class SupplierManager
    {
        private readonly SupplierRepository _repository;

        public SupplierManager()
        {
            _repository = new SupplierRepository();
        }

        public IEnumerable<Supplier> GetAll()
        {
            return _repository.GetAll();
        }

        public Supplier GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Supplier entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(Supplier entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Supplier existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.Name = entity.Name;
            existing.ContactName = entity.ContactName;
            existing.Email = entity.Email;
            existing.Phone = entity.Phone;
            existing.Address = entity.Address;
            existing.IsActive = entity.IsActive;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        // Pares Id / texto para llenar los dropdowns de las demas pantallas
        public IEnumerable<KeyValuePair<int, string>> GetOptions()
        {
            return _repository.Project(x => new { x.Id, x.Name })
                .Select(a => new KeyValuePair<int, string>(a.Id, a.Name))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
