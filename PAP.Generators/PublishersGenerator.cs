using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class PublishersGenerator : GeneratorBase<Publisher>
    {
        private static readonly string[] Names =
        {
            "Editorial Costa Rica", "Ediciones Sol y Luna", "Grupo Editorial Norma", "Editorial Planeta Azul",
            "Libros del Volcan", "Ediciones Faro", "Editorial Horizonte", "Casa del Libro Abierto",
            "Ediciones Pluma Verde", "Editorial Tinta Roja", "Ediciones del Puerto", "Editorial Alfa Centauro",
            "Ediciones La Ceiba", "Editorial Mar de Letras", "Ediciones Quetzal"
        };

        public PublishersGenerator()
            : base(new PublisherRepository())
        {
        }

        public override string EntityName
        {
            get { return "Publishers"; }
        }

        protected override IEnumerable<Publisher> Build()
        {
            DateTime now = DateTime.UtcNow;
            foreach (string name in Names)
            {
                string slug = DataFaker.Slug(name);
                yield return new Publisher
                {
                    Name = name,
                    Country = Fake.Country(),
                    Website = "https://www." + slug + ".com",
                    Email = "contacto@" + slug + ".com",
                    CreatedAt = now
                };
            }
        }
    }
}
