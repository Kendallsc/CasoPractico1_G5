using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class InventoryGenerator : GeneratorBase<Inventory>
    {
        public InventoryGenerator()
            : base(new InventoryRepository())
        {
        }

        // La tabla se llama dbo.Inventory (singular)
        public override string EntityName
        {
            get { return "Inventory"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Books" }; }
        }

        protected override IEnumerable<Inventory> Build()
        {
            var books = new BookRepository().Project(x => new { x.Id, x.IsInStock }).ToList();
            DateTime now = DateTime.UtcNow;

            // Exactamente un registro por libro (UQ_Inventory_Book); el stock respeta Books.IsInStock
            var items = new List<Inventory>();
            foreach (var book in books)
            {
                items.Add(new Inventory
                {
                    BookId = book.Id,
                    QuantityInStock = book.IsInStock ? Fake.Int(1, 150) : 0,
                    ReorderLevel = Fake.Pick(new[] { 5, 10, 15, 20 }),
                    LastRestockedAt = Fake.Chance(80) ? Fake.DateTimeBetween(now.AddMonths(-6), now) : (DateTime?)null
                });
            }
            return items;
        }
    }
}
