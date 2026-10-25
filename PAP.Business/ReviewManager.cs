using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Business
{
    public class ReviewManager
    {
        private readonly ReviewRepository _repository;

        public ReviewManager()
        {
            _repository = new ReviewRepository();
        }

        public IEnumerable<Review> GetAll()
        {
            return _repository.GetAll();
        }

        public Review GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(Review entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DateTime now = DateTime.UtcNow;
            entity.CreatedAt = now;

            _repository.Add(entity);
        }

        public void Update(Review entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Review existing = _repository.GetById(entity.Id);
            if (existing == null)
                throw new InvalidOperationException("El registro ya no existe.");

            existing.BookId = entity.BookId;
            existing.CustomerId = entity.CustomerId;
            existing.Rating = entity.Rating;
            existing.Comment = entity.Comment;

            _repository.Update(existing);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
