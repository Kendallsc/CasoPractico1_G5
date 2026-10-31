using PAP.DataAccess;
using PAP.Repositories;
using System.Collections.Generic;

namespace PAP.Generators
{
    public class RolesGenerator : GeneratorBase<Role>
    {
        // Admin, Manager y Cashier son los mismos que inserta el script de la base de datos
        private static readonly string[] Names = { "Admin", "Manager", "Cashier", "Warehouse", "Support" };

        public RolesGenerator()
            : base(new RoleRepository())
        {
        }

        public override string EntityName
        {
            get { return "Roles"; }
        }

        protected override IEnumerable<Role> Build()
        {
            foreach (string name in Names)
                yield return new Role { Name = name };
        }
    }
}
