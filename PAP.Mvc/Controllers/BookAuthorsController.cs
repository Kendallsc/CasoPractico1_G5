using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class BookAuthorsController : CrudControllerBase
    {
        private readonly BookAuthorManager _manager;

        public BookAuthorsController()
        {
            _manager = new BookAuthorManager();
        }

        public ActionResult Index()
        {
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            ViewBag.AuthorsOptions = ToOptions(new AuthorManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthor bookAuthor)
        {
            return Run(() => _manager.Add(bookAuthor), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int oldBookId, int oldAuthorId, int bookId, int authorId)
        {
            return Run(() => _manager.Replace(oldBookId, oldAuthorId, bookId, authorId), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int bookId, int authorId)
        {
            return Run(() => _manager.Delete(bookId, authorId), "Registro eliminado correctamente.");
        }
    }
}
