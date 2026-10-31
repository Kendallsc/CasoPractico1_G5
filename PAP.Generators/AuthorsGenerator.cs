using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class AuthorsGenerator : GeneratorBase<Author>
    {
        private const int Total = 40;

        public AuthorsGenerator()
            : base(new AuthorRepository())
        {
        }

        public override string EntityName
        {
            get { return "Authors"; }
        }

        protected override IEnumerable<Author> Build()
        {
            DateTime now = DateTime.UtcNow;
            for (int i = 0; i < Total; i++)
            {
                string first = Fake.FirstName();
                string last = Fake.LastName();
                string country = Fake.Country();

                yield return new Author
                {
                    FirstName = first,
                    LastName = last,
                    Country = country,
                    BirthDate = Fake.BirthDate(28, 85),
                    Biography = first + " " + last + " es un autor de " + country + ". " + Fake.Paragraph(2),
                    CreatedAt = now
                };
            }
        }
    }
}
