using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class RoleManager
    {
        private readonly RoleRepository _repository;

        public RoleManager()
        {
            _repository = new RoleRepository();
        }

        public IEnumerable<Role> GetAll()
        {
            return _repository.GetAll();
        }

        public Role GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Role entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(Role entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Role existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.Name = entity.Name;

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
