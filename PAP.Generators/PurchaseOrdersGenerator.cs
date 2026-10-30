using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class PurchaseOrdersGenerator : GeneratorBase<PurchaseOrder>
    {
        private const int Total = 30;

        private static readonly string[] Statuses = { "Pending", "Ordered", "Received", "Cancelled" };

        public PurchaseOrdersGenerator()
            : base(new PurchaseOrderRepository())
        {
        }

        public override string EntityName
        {
            get { return "PurchaseOrders"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Suppliers" }; }
        }

        protected override IEnumerable<PurchaseOrder> Build()
        {
            List<int> supplierIds = new SupplierRepository().Project(x => x.Id).ToList();
            DateTime now = DateTime.UtcNow;

            var items = new List<PurchaseOrder>();
            for (int i = 0; i < Total; i++)
            {
                DateTime date = Fake.DateTimeBetween(now.AddMonths(-8), now);

                // TotalCost queda en 0: PurchaseOrderItemsGenerator lo recalcula con las lineas reales
                items.Add(new PurchaseOrder
                {
                    SupplierId = Fake.Pick(supplierIds),
                    OrderDate = date,
                    ExpectedDate = date.Date.AddDays(Fake.Int(5, 30)),
                    Status = Fake.Pick(Statuses),
                    TotalCost = 0m
                });
            }
            return items;
        }
    }
}
