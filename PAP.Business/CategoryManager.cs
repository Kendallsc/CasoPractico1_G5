using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class CategoryManager
    {
        private readonly CategoryRepository _repository;

        public CategoryManager()
        {
            _repository = new CategoryRepository();
        }

        public IEnumerable<Category> GetAll()
        {
            return _repository.GetAll();
        }

        public Category GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            _repository.Add(entity);
        }

        public void Update(Category entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Category existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.Name = entity.Name;
            existing.Description = entity.Description;

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
