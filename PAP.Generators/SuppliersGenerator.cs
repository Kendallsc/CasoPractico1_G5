using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class SuppliersGenerator : GeneratorBase<Supplier>
    {
        private static readonly string[] Names =
        {
            "Distribuidora Literaria CR", "Importadora de Libros del Sur", "Papeleria y Libros Central",
            "Distribuciones El Lector", "Mayorista Paginas S.A.", "Libros al Por Mayor Ltda.",
            "Grupo Distribuidor Cultural", "Importaciones Biblos", "Logistica Editorial Tica",
            "Suministros El Escritor", "Distribuidora Atlas", "Comercial Letras Unidas"
        };

        public SuppliersGenerator()
            : base(new SupplierRepository())
        {
        }

        public override string EntityName
        {
            get { return "Suppliers"; }
        }

        protected override IEnumerable<Supplier> Build()
        {
            foreach (string name in Names)
            {
                string[] city = Fake.Pick(DataFaker.CostaRicaCities);
                yield return new Supplier
                {
                    Name = name,
                    ContactName = Fake.FirstName() + " " + Fake.LastName(),
                    Email = "ventas@" + DataFaker.Slug(name) + ".com",
                    Phone = Fake.Phone(),
                    Address = Fake.Pick(DataFaker.Streets) + ", " + city[0] + ", " + city[1],
                    IsActive = Fake.Chance(90)
                };
            }
        }
    }
}
