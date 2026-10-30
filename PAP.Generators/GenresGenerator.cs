using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class GenresGenerator : GeneratorBase<Genre>
    {
        // Nombres fijos: la columna Name tiene restriccion UNIQUE
        private static readonly string[][] Items =
        {
            new[] { "Misterio", "Intriga, crimenes y detectives." },
            new[] { "Ciencia ficcion", "Futuro, tecnologia y otros mundos." },
            new[] { "Fantasia", "Magia y mundos imaginarios." },
            new[] { "Romance", "Historias de amor." },
            new[] { "Terror", "Historias para dar miedo." },
            new[] { "Historico", "Relatos ambientados en el pasado." },
            new[] { "Aventura", "Viajes, exploracion y accion." },
            new[] { "Biografia", "La vida de personas reales." },
            new[] { "Poesia", "Obras en verso." },
            new[] { "Thriller", "Suspenso y tension." },
            new[] { "Drama", "Conflictos humanos y emocionales." },
            new[] { "Humor", "Lecturas comicas." }
        };

        public GenresGenerator()
            : base(new GenreRepository())
        {
        }

        public override string EntityName
        {
            get { return "Genres"; }
        }

        protected override IEnumerable<Genre> Build()
        {
            foreach (string[] item in Items)
                yield return new Genre { Name = item[0], Description = item[1] };
        }
    }
}
