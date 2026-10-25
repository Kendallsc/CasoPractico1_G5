using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class PublisherManager
    {
        private readonly PublisherRepository _repository;

        public PublisherManager()
        {
            _repository = new PublisherRepository();
        }

        public IEnumerable<Publisher> GetAll()
        {
            return _repository.GetAll();
        }

        public Publisher GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Publisher entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;

            _repository.Add(entity);
        }

        public void Update(Publisher entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Publisher existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.Name = entity.Name;
            existing.Country = entity.Country;
            existing.Website = entity.Website;
            existing.Email = entity.Email;

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
