using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class ReviewsGenerator : GeneratorBase<Review>
    {
        private const int Total = 200;

        private static readonly string[] Comments =
        {
            "Excelente libro, lo recomiendo.", "Me gusto mucho la historia.", "Buena lectura, aunque el final fue predecible.",
            "No era lo que esperaba.", "Muy bien escrito, lo lei en dos dias.", "Regular, le falto profundidad.",
            "Una obra imprescindible.", "Los personajes estan muy bien construidos.", "Algo lento al inicio, pero vale la pena.",
            "No lo terminaria de nuevo."
        };

        public ReviewsGenerator()
            : base(new ReviewRepository())
        {
        }

        public override string EntityName
        {
            get { return "Reviews"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Books", "Customers" }; }
        }

        protected override IEnumerable<Review> Build()
        {
            List<int> bookIds = new BookRepository().Project(x => x.Id).ToList();
            List<int> customerIds = new CustomerRepository().Project(x => x.Id).ToList();
            DateTime now = DateTime.UtcNow;

            // Un cliente solo puede resenar un libro una vez (UQ_Reviews_Book_Customer)
            int target = Math.Min(Total, bookIds.Count * customerIds.Count);
            var used = new HashSet<Tuple<int, int>>();
            var items = new List<Review>();
            while (items.Count < target)
            {
                var key = Tuple.Create(Fake.Pick(bookIds), Fake.Pick(customerIds));
                if (!used.Add(key))
                    continue;

                byte rating = (byte)Fake.Int(1, 5);
                items.Add(new Review
                {
                    BookId = key.Item1,
                    CustomerId = key.Item2,
                    Rating = rating,
                    Comment = Fake.Chance(85) ? Fake.Pick(Comments) : null,
                    CreatedAt = Fake.DateTimeBetween(now.AddYears(-1), now)
                });
            }
            return items;
        }
    }
}
