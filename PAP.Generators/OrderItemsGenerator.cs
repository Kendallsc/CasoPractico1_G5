using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class OrderItemsGenerator : GeneratorBase<OrderItem>
    {
        // Impuesto al valor agregado de Costa Rica
        private const decimal TaxRate = 0.13m;

        public OrderItemsGenerator()
            : base(new OrderItemRepository())
        {
        }

        public override string EntityName
        {
            get { return "OrderItems"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Orders", "Books" }; }
        }

        protected override IEnumerable<OrderItem> Build()
        {
            List<int> orderIds = new OrderRepository().Project(x => x.Id).ToList();
            var books = new BookRepository().Project(x => new { x.Id, x.Price }).ToList();

            var items = new List<OrderItem>();
            foreach (int orderId in orderIds)
            {
                // De 1 a 4 libros distintos por orden
                foreach (var book in Fake.PickMany(books, Fake.Int(1, 4)))
                {
                    items.Add(new OrderItem
                    {
                        OrderId = orderId,
                        BookId = book.Id,
                        Quantity = Fake.Int(1, 3),
                        UnitPrice = book.Price,
                        Discount = Fake.Chance(20) ? Math.Round(book.Price * 0.10m, 2) : 0m
                    });
                }
            }
            return items;
        }

        // Con las lineas ya insertadas se recalculan Subtotal, impuesto y Total de cada orden
        protected override void AfterGenerate(List<OrderItem> items)
        {
            Dictionary<int, decimal> subtotals = items
                .GroupBy(x => x.OrderId)
                .ToDictionary(g => g.Key, g => g.Sum(x => (x.UnitPrice - x.Discount) * x.Quantity));

            var orderRepository = new OrderRepository();
            foreach (Order order in orderRepository.GetAll())
            {
                decimal subtotal;
                if (!subtotals.TryGetValue(order.Id, out subtotal))
                    continue;

                order.Subtotal = subtotal;
                order.TaxAmount = Math.Round(subtotal * TaxRate, 2);
                order.Total = order.Subtotal + order.TaxAmount + order.ShippingCost;
            }
            orderRepository.Save();
        }
    }
}
