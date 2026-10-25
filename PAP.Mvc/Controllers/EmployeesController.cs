using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class EmployeesController : CrudControllerBase
    {
        private readonly EmployeeManager _manager;

        public EmployeesController()
        {
            _manager = new EmployeeManager();
        }

        public ActionResult Index()
        {
            ViewBag.RolesOptions = ToOptions(new RoleManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, string password)
        {
            return Run(() => _manager.Add(employee, password), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee, string password)
        {
            return Run(() => _manager.Update(employee, password), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
