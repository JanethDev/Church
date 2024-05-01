using church.backend.Models.catalogue.roles;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models.register;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

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
