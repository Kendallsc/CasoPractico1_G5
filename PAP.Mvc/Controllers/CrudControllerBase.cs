using PAP.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Base comun de los controllers CRUD: ejecuta la accion, guarda el mensaje de exito / error
    /// en TempData y regresa siempre al Index (la unica vista de cada entidad).
    /// </summary>
    public abstract class CrudControllerBase : Controller
    {
        protected ActionResult Run(Action action, string successMessage)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = BuildModelErrors();
                return RedirectToAction("Index");
            }

            try
            {
                action();
                TempData["Success"] = successMessage;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ErrorTranslator.Translate(ex);
            }

            return RedirectToAction("Index");
        }

        protected static List<SelectListItem> ToOptions(IEnumerable<KeyValuePair<int, string>> items)
        {
            return items
                .Select(i => new SelectListItem { Value = i.Key.ToString(), Text = i.Value })
                .ToList();
        }

        private string BuildModelErrors()
        {
            var sb = new StringBuilder("Datos no validos: ");
            foreach (var entry in ModelState.Where(m => m.Value.Errors.Count > 0))
            {
                foreach (var error in entry.Value.Errors)
                {
                    string message = string.IsNullOrEmpty(error.ErrorMessage) && error.Exception != null
                        ? "valor con formato incorrecto"
                        : error.ErrorMessage;
                    sb.Append("[" + entry.Key + "] " + message + " ");
                }
            }
            return sb.ToString().Trim();
        }
    }
}
