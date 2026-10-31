using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    /// <summary>
    /// Estado de una tabla para mostrar en la pantalla de Admin.
    /// </summary>
    public class GeneratorStatus
    {
        public string EntityName { get; set; }
        public string TableName { get; set; }
        public int Count { get; set; }
        public List<string> Dependencies { get; set; }

        /// <summary>
        /// Dependencias que todavia no tienen datos (si hay alguna no se puede generar).
        /// </summary>
        public List<string> MissingDependencies { get; set; }

        public bool HasData
        {
            get { return Count > 0; }
        }
    }

    /// <summary>
    /// Punto de entrada de la generacion: conoce todos los generadores y aplica las reglas
    /// (no generar si la tabla ya tiene datos y respetar las llaves foraneas).
    /// </summary>
    public class GeneratorService
    {
        private readonly List<IGenerator> _generators;

        public GeneratorService()
        {
            // Mismo orden en el que aparecen las tablas en el enunciado
            _generators = new List<IGenerator>
            {
                new AddressesGenerator(),
                new AuthorsGenerator(),
                new BookAuthorsGenerator(),
                new BooksGenerator(),
                new CartItemsGenerator(),
                new CategoriesGenerator(),
                new CustomersGenerator(),
                new EmployeesGenerator(),
                new GenresGenerator(),
                new InventoryGenerator(),
                new OrderItemsGenerator(),
                new OrdersGenerator(),
                new PaymentsGenerator(),
                new PublishersGenerator(),
                new PurchaseOrderItemsGenerator(),
                new PurchaseOrdersGenerator(),
                new ReviewsGenerator(),
                new RolesGenerator(),
                new ShoppingCartsGenerator(),
                new SuppliersGenerator()
            };
        }

        public IEnumerable<GeneratorStatus> GetStatus()
        {
            Dictionary<string, int> counts = _generators.ToDictionary(g => g.EntityName, g => g.Count());

            return _generators
                .Select(g => new GeneratorStatus
                {
                    EntityName = g.EntityName,
                    TableName = g.TableName,
                    Count = counts[g.EntityName],
                    Dependencies = g.Dependencies.ToList(),
                    MissingDependencies = g.Dependencies.Where(d => counts[d] == 0).ToList()
                })
                .ToList();
        }

        /// <summary>
        /// Genera los datos de una tabla.
        /// </summary>
        /// <returns>Cantidad de registros insertados</returns>
        /// <exception cref="InvalidOperationException">Si la tabla ya tiene datos o le faltan datos a una dependencia</exception>
        public int Generate(string entityName)
        {
            IGenerator generator = Find(entityName);

            int existing = generator.Count();
            if (existing > 0)
                throw new InvalidOperationException(string.Format(
                    "La tabla {0} ya tiene {1} registro(s). No se generaron datos.", generator.TableName, existing));

            List<string> missing = generator.Dependencies.Where(d => Find(d).Count() == 0).ToList();
            if (missing.Any())
                throw new InvalidOperationException(string.Format(
                    "No se puede generar {0}: primero debe generar {1}.",
                    generator.TableName, string.Join(", ", missing.Select(d => "dbo." + d))));

            return generator.Generate();
        }

        public string GetTableName(string entityName)
        {
            return Find(entityName).TableName;
        }

        private IGenerator Find(string entityName)
        {
            IGenerator generator = _generators.FirstOrDefault(g =>
                string.Equals(g.EntityName, entityName, StringComparison.OrdinalIgnoreCase));

            if (generator == null)
                throw new ArgumentException("No existe un generador para la entidad '" + entityName + "'.");

            return generator;
        }
    }
}
