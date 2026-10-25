using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class CustomersController : CrudControllerBase
    {
        private readonly CustomerManager _manager;

        public CustomersController()
        {
            _manager = new CustomerManager();
        }

        public ActionResult Index()
        {
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer, string password)
        {
            return Run(() => _manager.Add(customer, password), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer, string password)
        {
            return Run(() => _manager.Update(customer, password), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
