using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class PaymentsController : CrudControllerBase
    {
        private readonly PaymentManager _manager;

        public PaymentsController()
        {
            _manager = new PaymentManager();
        }

        public ActionResult Index()
        {
            ViewBag.OrdersOptions = ToOptions(new OrderManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payment payment)
        {
            return Run(() => _manager.Add(payment), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payment payment)
        {
            return Run(() => _manager.Update(payment), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
