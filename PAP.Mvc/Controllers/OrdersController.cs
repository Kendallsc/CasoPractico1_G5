using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class OrdersController : CrudControllerBase
    {
        private readonly OrderManager _manager;

        public OrdersController()
        {
            _manager = new OrderManager();
        }

        public ActionResult Index()
        {
            ViewBag.CustomersOptions = ToOptions(new CustomerManager().GetOptions());
            ViewBag.AddressesOptions = ToOptions(new AddressManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            return Run(() => _manager.Add(order), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            return Run(() => _manager.Update(order), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
