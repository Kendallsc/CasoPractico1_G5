using PAP.DataAccess;
using PAP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class ShoppingCartsGenerator : GeneratorBase<ShoppingCart>
    {
        public ShoppingCartsGenerator()
            : base(new ShoppingCartRepository())
        {
        }

        public override string EntityName
        {
            get { return "ShoppingCarts"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Customers" }; }
        }

        protected override IEnumerable<ShoppingCart> Build()
        {
            List<int> customerIds = new CustomerRepository().Project(x => x.Id).ToList();
            DateTime now = DateTime.UtcNow;

            // Aproximadamente la mitad de los clientes tiene un carrito abierto (uno por cliente)
            var items = new List<ShoppingCart>();
            foreach (int customerId in Fake.PickMany(customerIds, customerIds.Count / 2 + 1))
            {
                DateTime created = Fake.DateTimeBetween(now.AddDays(-30), now);
                items.Add(new ShoppingCart
                {
                    CustomerId = customerId,
                    CreatedAt = created,
                    UpdatedAt = created
                });
            }
            return items;
        }
    }
}
