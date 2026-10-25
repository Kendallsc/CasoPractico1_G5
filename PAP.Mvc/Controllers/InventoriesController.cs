using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class InventoriesController : CrudControllerBase
    {
        private readonly InventoryManager _manager;

        public InventoriesController()
        {
            _manager = new InventoryManager();
        }

        public ActionResult Index()
        {
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory)
        {
            return Run(() => _manager.Add(inventory), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inventory inventory)
        {
            return Run(() => _manager.Update(inventory), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
