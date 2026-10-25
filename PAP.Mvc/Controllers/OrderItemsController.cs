using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class OrderItemsController : CrudControllerBase
    {
        private readonly OrderItemManager _manager;

        public OrderItemsController()
        {
            _manager = new OrderItemManager();
        }

        public ActionResult Index()
        {
            ViewBag.OrdersOptions = ToOptions(new OrderManager().GetOptions());
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderItem orderItem)
        {
            return Run(() => _manager.Add(orderItem), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderItem orderItem)
        {
            return Run(() => _manager.Update(orderItem), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
