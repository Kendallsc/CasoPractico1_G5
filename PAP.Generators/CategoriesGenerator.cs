using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class CategoriesGenerator : GeneratorBase<Category>
    {
        // Nombres fijos: la columna Name tiene restriccion UNIQUE
        private static readonly string[][] Items =
        {
            new[] { "Ficcion", "Novelas y relatos de imaginacion." },
            new[] { "No ficcion", "Obras basadas en hechos reales." },
            new[] { "Infantil", "Libros para ninos y ninas." },
            new[] { "Juvenil", "Lecturas para adolescentes." },
            new[] { "Academico", "Textos universitarios y de estudio." },
            new[] { "Tecnologia", "Programacion, informatica y ciencia aplicada." },
            new[] { "Negocios", "Administracion, finanzas y emprendimiento." },
            new[] { "Autoayuda", "Desarrollo personal y bienestar." },
            new[] { "Arte", "Pintura, fotografia, musica y diseno." },
            new[] { "Cocina", "Recetas y gastronomia." }
        };

        public CategoriesGenerator()
            : base(new CategoryRepository())
        {
        }

        public override string EntityName
        {
            get { return "Categories"; }
        }

        protected override IEnumerable<Category> Build()
        {
            foreach (string[] item in Items)
                yield return new Category { Name = item[0], Description = item[1] };
        }
    }
}
