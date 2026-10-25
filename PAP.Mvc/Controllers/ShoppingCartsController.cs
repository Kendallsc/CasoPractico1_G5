using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class ShoppingCartsController : CrudControllerBase
    {
        private readonly ShoppingCartManager _manager;

        public ShoppingCartsController()
        {
            _manager = new ShoppingCartManager();
        }

        public ActionResult Index()
        {
            ViewBag.CustomersOptions = ToOptions(new CustomerManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShoppingCart shoppingCart)
        {
            return Run(() => _manager.Add(shoppingCart), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShoppingCart shoppingCart)
        {
            return Run(() => _manager.Update(shoppingCart), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
