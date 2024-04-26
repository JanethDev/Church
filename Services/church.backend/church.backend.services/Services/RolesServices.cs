using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models.register;
using church.backend.services.Models.roles;
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
