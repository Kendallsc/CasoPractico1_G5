using PAP.Generators;
using PAP.Mvc.Helpers;
using System;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    /// <summary>
    /// Pantalla de administracion: desde aqui se generan los datos de prueba de cada tabla.
    /// (Por ahora cualquiera puede accesarla.)
    /// </summary>
    public class AdminController : Controller
    {
        private readonly GeneratorService _service;

        public AdminController()
        {
            _service = new GeneratorService();
        }

        public ActionResult Index()
        {
            return View(_service.GetStatus());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(string entity)
        {
            try
            {
                int inserted = _service.Generate(entity);
                TempData["Success"] = string.Format("Se generaron {0} registro(s) en {1}.", inserted, _service.GetTableName(entity));
            }
            catch (InvalidOperationException ex)
            {
                // Reglas de la generacion: la tabla ya tiene datos o falta generar una dependencia
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = ErrorTranslator.Translate(ex);
            }

            return RedirectToAction("Index");
        }
    }
}
