using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class BooksController : CrudControllerBase
    {
        private readonly BookManager _manager;

        public BooksController()
        {
            _manager = new BookManager();
        }

        public ActionResult Index()
        {
            ViewBag.PublishersOptions = ToOptions(new PublisherManager().GetOptions());
            ViewBag.CategoriesOptions = ToOptions(new CategoryManager().GetOptions());
            ViewBag.GenresOptions = ToOptions(new GenreManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            return Run(() => _manager.Add(book), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            return Run(() => _manager.Update(book), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
