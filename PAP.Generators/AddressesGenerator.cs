using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace PAP.Generators
{
    public class AddressesGenerator : GeneratorBase<Address>
    {
        public AddressesGenerator()
            : base(new AddressRepository())
        {
        }

        public override string EntityName
        {
            get { return "Addresses"; }
        }

        public override IEnumerable<string> Dependencies
        {
            get { return new[] { "Customers" }; }
        }

        protected override IEnumerable<Address> Build()
        {
            List<int> customerIds = new CustomerRepository().Project(x => x.Id).ToList();

            // Cada cliente tiene una direccion de envio por defecto y, a veces, una de facturacion
            var items = new List<Address>();
            foreach (int customerId in customerIds)
            {
                items.Add(NewAddress(customerId, "Shipping", true));
                if (Fake.Chance(40))
                    items.Add(NewAddress(customerId, "Billing", false));
            }
            return items;
        }

        private Address NewAddress(int customerId, string type, bool isDefault)
        {
            string[] city = Fake.Pick(DataFaker.CostaRicaCities);
            return new Address
            {
                CustomerId = customerId,
                AddressType = type,
                Street = Fake.Pick(DataFaker.Streets) + ", casa " + Fake.Int(1, 250),
                City = city[0],
                State = city[1],
                PostalCode = Fake.PostalCode(),
                Country = "Costa Rica",
                IsDefault = isDefault
            };
        }
    }
}
