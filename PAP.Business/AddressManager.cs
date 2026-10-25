using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class AddressManager
    {
        private readonly AddressRepository _repository;

        public AddressManager()
        {
            _repository = new AddressRepository();
        }

        public IEnumerable<Address> GetAll()
        {
            return _repository.GetAll();
        }

        public Address GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(Address entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Address existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.CustomerId = entity.CustomerId;
            existing.AddressType = entity.AddressType;
            existing.Street = entity.Street;
            existing.City = entity.City;
            existing.State = entity.State;
            existing.PostalCode = entity.PostalCode;
            existing.Country = entity.Country;
            existing.IsDefault = entity.IsDefault;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        // Pares Id / texto para llenar los dropdowns de las demas pantallas
        public IEnumerable<KeyValuePair<int, string>> GetOptions()
        {
            return _repository.Project(x => new { x.Id, x.Street, x.City })
                .Select(a => new KeyValuePair<int, string>(a.Id, "#" + a.Id + " - " + a.Street + ", " + a.City))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
