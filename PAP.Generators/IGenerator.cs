using System.Collections.Generic;

namespace PAP.Generators
{
    /// <summary>
    /// Contrato comun de todos los generadores. Permite tratarlos igual desde el AdminController.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Nombre de la entidad / tabla sin esquema (ej. "Authors"). Se usa como identificador del generador.
        /// </summary>
        string EntityName { get; }

        /// <summary>
        /// Nombre completo de la tabla (ej. "dbo.Authors").
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Entidades que deben tener datos antes de poder generar esta (llaves foraneas).
        /// </summary>
        IEnumerable<string> Dependencies { get; }

        /// <summary>
        /// Cantidad de registros que tiene la tabla actualmente.
        /// </summary>
        int Count();

        /// <summary>
        /// Genera los registros y los inserta usando el repository de la entidad.
        /// </summary>
        /// <returns>Cantidad de registros insertados</returns>
        int Generate();
    }
}
