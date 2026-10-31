using PAP.Business;
using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class EmployeesGenerator : GeneratorBase<Employee>
    {
        private const int Total = 15;

        // Todos los empleados generados comparten esta clave (se guarda hasheada con PBKDF2)
        public const string DefaultPassword = "Empleado123!";

        public EmployeesGenerator()
            : base(new EmployeeRepository())
        {
        }

        public override string EntityName
        {
            get { return "Employees"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Roles" }; }
        }

        protected override IEnumerable<Employee> Build()
        {
            List<int> roleIds = new RoleRepository().Project(x => x.Id).ToList();
            DateTime now = DateTime.UtcNow;

            var items = new List<Employee>();
            for (int i = 1; i <= Total; i++)
            {
                string first = Fake.FirstName();
                string last = Fake.LastName();
                items.Add(new Employee
                {
                    RoleId = Fake.Pick(roleIds),
                    FirstName = first,
                    LastName = last,
                    Email = Fake.Email(first, last, i, "bookstore.com"),
                    PasswordHash = PasswordHasher.Hash(DefaultPassword),
                    IsActive = Fake.Chance(90),
                    CreatedAt = Fake.DateTimeBetween(now.AddYears(-3), now)
                });
            }
            return items;
        }
    }
}
