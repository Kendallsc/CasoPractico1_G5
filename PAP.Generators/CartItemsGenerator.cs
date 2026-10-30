using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class CartItemsGenerator : GeneratorBase<CartItem>
    {
        public CartItemsGenerator()
            : base(new CartItemRepository())
        {
        }

        public override string EntityName
        {
            get { return "CartItems"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "ShoppingCarts", "Books" }; }
        }

        protected override IEnumerable<CartItem> Build()
        {
            var carts = new ShoppingCartRepository().Project(x => new { x.Id, x.CreatedAt }).ToList();
            List<int> bookIds = new BookRepository().Project(x => x.Id).ToList();
            DateTime now = DateTime.UtcNow;

            // PickMany no repite libros, asi se respeta UQ_CartItems_Cart_Book (CartId, BookId)
            var items = new List<CartItem>();
            foreach (var cart in carts)
            {
                foreach (int bookId in Fake.PickMany(bookIds, Fake.Int(1, 4)))
                {
                    items.Add(new CartItem
                    {
                        CartId = cart.Id,
                        BookId = bookId,
                        Quantity = Fake.Int(1, 3),
                        AddedAt = Fake.DateTimeBetween(cart.CreatedAt, now)
                    });
                }
            }
            return items;
        }
    }
}
