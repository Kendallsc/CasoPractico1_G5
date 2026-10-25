using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class AddressesController : CrudControllerBase
    {
        private readonly AddressManager _manager;

        public AddressesController()
        {
            _manager = new AddressManager();
        }

        public ActionResult Index()
        {
            ViewBag.CustomersOptions = ToOptions(new CustomerManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Address address)
        {
            return Run(() => _manager.Add(address), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Address address)
        {
            return Run(() => _manager.Update(address), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
