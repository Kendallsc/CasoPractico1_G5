using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class PurchaseOrderItemsGenerator : GeneratorBase<PurchaseOrderItem>
    {
        public PurchaseOrderItemsGenerator()
            : base(new PurchaseOrderItemRepository())
        {
        }

        public override string EntityName
        {
            get { return "PurchaseOrderItems"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "PurchaseOrders", "Books" }; }
        }

        protected override IEnumerable<PurchaseOrderItem> Build()
        {
            List<int> purchaseOrderIds = new PurchaseOrderRepository().Project(x => x.Id).ToList();
            var books = new BookRepository().Project(x => new { x.Id, x.Price }).ToList();

            var items = new List<PurchaseOrderItem>();
            foreach (int purchaseOrderId in purchaseOrderIds)
            {
                foreach (var book in Fake.PickMany(books, Fake.Int(2, 6)))
                {
                    // El costo al proveedor es entre 50% y 70% del precio de venta
                    decimal factor = Fake.Int(50, 70) / 100m;
                    items.Add(new PurchaseOrderItem
                    {
                        PurchaseOrderId = purchaseOrderId,
                        BookId = book.Id,
                        Quantity = Fake.Int(5, 50),
                        UnitCost = Math.Round(book.Price * factor, 2)
                    });
                }
            }
            return items;
        }

        // Con las lineas ya insertadas se recalcula el TotalCost de cada orden de compra
        protected override void AfterGenerate(List<PurchaseOrderItem> items)
        {
            Dictionary<int, decimal> totals = items
                .GroupBy(x => x.PurchaseOrderId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.UnitCost * x.Quantity));

            var purchaseOrderRepository = new PurchaseOrderRepository();
            foreach (PurchaseOrder purchaseOrder in purchaseOrderRepository.GetAll())
            {
                decimal total;
                if (totals.TryGetValue(purchaseOrder.Id, out total))
                    purchaseOrder.TotalCost = total;
            }
            purchaseOrderRepository.Save();
        }
    }
}
