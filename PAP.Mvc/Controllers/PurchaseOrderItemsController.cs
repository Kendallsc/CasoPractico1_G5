using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class PurchaseOrderItemsController : CrudControllerBase
    {
        private readonly PurchaseOrderItemManager _manager;

        public PurchaseOrderItemsController()
        {
            _manager = new PurchaseOrderItemManager();
        }

        public ActionResult Index()
        {
            ViewBag.PurchaseOrdersOptions = ToOptions(new PurchaseOrderManager().GetOptions());
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseOrderItem purchaseOrderItem)
        {
            return Run(() => _manager.Add(purchaseOrderItem), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PurchaseOrderItem purchaseOrderItem)
        {
            return Run(() => _manager.Update(purchaseOrderItem), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
