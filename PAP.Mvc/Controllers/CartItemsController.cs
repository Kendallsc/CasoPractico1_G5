using PAP.Business;
using PAP.DataAccess;
using System.Web.Mvc;

namespace PAP.Mvc.Controllers
{
    public class CartItemsController : CrudControllerBase
    {
        private readonly CartItemManager _manager;

        public CartItemsController()
        {
            _manager = new CartItemManager();
        }

        public ActionResult Index()
        {
            ViewBag.ShoppingCartsOptions = ToOptions(new ShoppingCartManager().GetOptions());
            ViewBag.BooksOptions = ToOptions(new BookManager().GetOptions());
            return View(_manager.GetAll());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CartItem cartItem)
        {
            return Run(() => _manager.Add(cartItem), "Registro creado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CartItem cartItem)
        {
            return Run(() => _manager.Update(cartItem), "Registro actualizado correctamente.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            return Run(() => _manager.Delete(id), "Registro eliminado correctamente.");
        }
    }
}
