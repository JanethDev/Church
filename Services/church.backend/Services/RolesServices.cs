using church.backend.Models.catalogue.roles;
using church.backend.services.DataBase;

namespace church.backend.services.Services
{
    public class RolesServices
    {
        private readonly rolesDB _rolesDB;

        public RolesServices(IConfiguration configuration, rolesDB rolesDB)
        {
            _rolesDB = rolesDB;
        }

        public roles_response consultRoles()
        {
            return _rolesDB.consultRoles();
        }
    }
}
