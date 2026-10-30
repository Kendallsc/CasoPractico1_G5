using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class OrdersGenerator : GeneratorBase<Order>
    {
        private const int Total = 120;

        public static readonly string[] Statuses = { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

        public OrdersGenerator()
            : base(new OrderRepository())
        {
        }

        public override string EntityName
        {
            get { return "Orders"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Customers", "Addresses" }; }
        }

        protected override IEnumerable<Order> Build()
        {
            // La direccion de envio debe pertenecer al mismo cliente de la orden
            Dictionary<int, List<int>> addressesByCustomer = new AddressRepository()
                .Project(x => new { x.Id, x.CustomerId })
                .GroupBy(a => a.CustomerId)
                .ToDictionary(g => g.Key, g => g.Select(a => a.Id).ToList());

            List<int> customerIds = addressesByCustomer.Keys.ToList();
            DateTime now = DateTime.UtcNow;

            var items = new List<Order>();
            for (int i = 0; i < Total; i++)
            {
                int customerId = Fake.Pick(customerIds);
                DateTime date = Fake.DateTimeBetween(now.AddMonths(-12), now);
                decimal shipping = Fake.Pick(new[] { 0m, 2.5m, 3.5m, 5m });

                // Subtotal e impuesto quedan en 0: OrderItemsGenerator los recalcula con las lineas reales
                items.Add(new Order
                {
                    CustomerId = customerId,
                    ShippingAddressId = Fake.Pick(addressesByCustomer[customerId]),
                    OrderDate = date,
                    Status = Fake.Pick(Statuses),
                    Subtotal = 0m,
                    TaxAmount = 0m,
                    ShippingCost = shipping,
                    Total = shipping,
                    Notes = Fake.Chance(25) ? Fake.Sentence(5, 10) : null,
                    UpdatedAt = date
                });
            }
            return items;
        }
    }
}
