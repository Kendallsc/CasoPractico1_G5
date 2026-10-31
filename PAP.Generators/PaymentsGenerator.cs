using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class PaymentsGenerator : GeneratorBase<Payment>
    {
        private static readonly string[] Methods = { "CreditCard", "DebitCard", "SINPE", "PayPal", "Cash" };

        public PaymentsGenerator()
            : base(new PaymentRepository())
        {
        }

        public override string EntityName
        {
            get { return "Payments"; }
        }

        // Depende de OrderItems para que el monto pagado sea el Total final de la orden
        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Orders", "OrderItems" }; }
        }

        protected override IEnumerable<Payment> Build()
        {
            var orders = new OrderRepository()
                .Project(x => new { x.Id, x.Total, x.Status, x.OrderDate })
                .ToList();

            var items = new List<Payment>();
            foreach (var order in orders)
            {
                // Las ordenes pendientes todavia no se han pagado
                if (order.Status == "Pending")
                    continue;

                string method = Fake.Pick(Methods);
                items.Add(new Payment
                {
                    OrderId = order.Id,
                    PaymentMethod = method,
                    Amount = order.Total,
                    PaymentDate = order.OrderDate.AddMinutes(Fake.Int(1, 120)),
                    Status = order.Status == "Cancelled" ? "Refunded" : "Completed",
                    TransactionRef = method == "Cash" ? null : Fake.Reference("TX")
                });
            }
            return items;
        }
    }
}
