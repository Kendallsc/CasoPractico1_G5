using System;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;

namespace PAP.Mvc.Helpers
{
    /// <summary>
    /// Convierte excepciones de EF / SQL Server en mensajes entendibles para mostrar en pantalla.
    /// </summary>
    public static class ErrorTranslator
    {
        public static string Translate(Exception ex)
        {
            var validation = ex as DbEntityValidationException;
            if (validation != null)
            {
                var messages = validation.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => e.ErrorMessage)
                    .Distinct()
                    .ToList();
                return "Datos no validos: " + string.Join(" ", messages);
            }

            Exception current = ex;
            while (current != null)
            {
                var sql = current as SqlException;
                if (sql != null)
                {
                    switch (sql.Number)
                    {
                        case 547:
                            if (sql.Message.IndexOf("DELETE", StringComparison.OrdinalIgnoreCase) >= 0)
                                return "No se puede eliminar: el registro tiene datos relacionados en otras tablas.";
                            return "La base de datos rechazo el dato (llave foranea inexistente o valor fuera de rango).";
                        case 2601:
                        case 2627:
                            return "Ya existe un registro con ese valor unico (por ejemplo correo, nombre o ISBN).";
                    }
                }
                current = current.InnerException;
            }

            Exception inner = ex;
            while (inner.InnerException != null)
                inner = inner.InnerException;
            return inner.Message;
        }
    }
}
