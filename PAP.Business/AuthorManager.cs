using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class AuthorManager
    {
        private readonly AuthorRepository _repository;

        public AuthorManager()
        {
            _repository = new AuthorRepository();
        }

        public IEnumerable<Author> GetAll()
        {
            return _repository.GetAll();
        }

        public Author GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;

            _repository.Add(entity);
        }

        public void Update(Author entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Author existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.FirstName = entity.FirstName;
            existing.LastName = entity.LastName;
            existing.Biography = entity.Biography;
            existing.BirthDate = entity.BirthDate;
            existing.Country = entity.Country;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        // Pares Id / texto para llenar los dropdowns de las demas pantallas
        public IEnumerable<KeyValuePair<int, string>> GetOptions()
        {
            return _repository.Project(x => new { x.Id, x.FirstName, x.LastName })
                .Select(a => new KeyValuePair<int, string>(a.Id, a.FirstName + " " + a.LastName))
                .OrderBy(o => o.Value)
                .ToList();
        }
    }
}
