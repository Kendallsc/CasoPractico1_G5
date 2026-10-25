using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class PurchaseOrdersController : CrudControllerBase
    {
        private readonly PurchaseOrderManager _manager;

        public PurchaseOrdersController()
        {
            _manager = new PurchaseOrderManager();
        }

        public ActionResult Index()
        {
            ViewBag.SuppliersOptions = ToOptions(new SupplierManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PurchaseOrder purchaseOrder)
        {
            return Run(() => _manager.Add(purchaseOrder), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PurchaseOrder purchaseOrder)
        {
            return Run(() => _manager.Update(purchaseOrder), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
