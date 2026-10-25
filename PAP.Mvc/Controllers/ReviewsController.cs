using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class ReviewsController : CrudControllerBase
    {
        private readonly ReviewManager _manager;

        public ReviewsController()
        {
            _manager = new ReviewManager();
        }

        public ActionResult Index()
        {
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            ViewBag.CustomersOptions = ToOptions(new CustomerManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Review review)
        {
            return Run(() => _manager.Add(review), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Review review)
        {
            return Run(() => _manager.Update(review), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
