using PAP.Business;
using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class CustomersGenerator : GeneratorBase<Customer>
    {
        private const int Total = 60;

        // Todos los clientes generados comparten esta clave (se guarda hasheada con PBKDF2)
        public const string DefaultPassword = "Cliente123!";

        public CustomersGenerator()
            : base(new CustomerRepository())
        {
        }

        public override string EntityName
        {
            get { return "Customers"; }
        }

        protected override IEnumerable<Customer> Build()
        {
            DateTime now = DateTime.UtcNow;
            for (int i = 1; i <= Total; i++)
            {
                string first = Fake.FirstName();
                string last = Fake.LastName();
                DateTime created = Fake.DateTimeBetween(now.AddYears(-2), now);

                yield return new Customer
                {
                    FirstName = first,
                    LastName = last,
                    Email = Fake.Email(first, last, i, "cliente.com"),
                    PasswordHash = PasswordHasher.Hash(DefaultPassword),
                    Phone = Fake.Phone(),
                    BirthDate = Fake.BirthDate(18, 75),
                    IsActive = Fake.Chance(92),
                    CreatedAt = created,
                    UpdatedAt = created
                };
            }
        }
    }
}
