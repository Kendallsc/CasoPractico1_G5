using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    /// <summary>
    /// Base de los generadores: cada clase hija solo construye la lista de entidades (Build)
    /// y la base se encarga de insertarlas en masa con el repository de la entidad.
    /// </summary>
    /// <typeparam name="T">Entidad del data model que se genera</typeparam>
    public abstract class GeneratorBase<T> : IGenerator where T : class
    {
        protected readonly RepositoryBase<T> Repository;
        protected readonly DataFaker Fake;

        protected GeneratorBase(RepositoryBase<T> repository)
        {
            Repository = repository;
            Fake = new DataFaker();
        }

        public abstract string EntityName { get; }

        public string TableName
        {
            get { return "dbo." + EntityName; }
        }

        public virtual IEnumerable<string> Dependencies
        {
            get { return new string[0]; }
        }

        public int Count()
        {
            return Repository.Count();
        }

        public int Generate()
        {
            List<T> items = Build().ToList();
            Repository.AddRange(items);
            AfterGenerate(items);
            return items.Count;
        }

        /// <summary>
        /// Construye los registros a insertar (sin guardarlos).
        /// </summary>
        protected abstract IEnumerable<T> Build();

        /// <summary>
        /// Punto opcional para ajustar otras tablas despues de insertar (ej. recalcular totales).
        /// </summary>
        protected virtual void AfterGenerate(List<T> items)
        {
        }
    }
}
