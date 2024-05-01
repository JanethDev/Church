<<<<<<< HEAD:Services/church.backend/Services/RolesServices.cs
﻿using church.backend.Models.catalogue.roles;
using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models.register;
=======
﻿using church.backend.services.DataBase;
using church.backend.services.JsonWebToken;
using church.backend.services.Models.register;
using church.backend.services.Models.roles;
>>>>>>> 1d3f015b43dff82c1574dde5dc46765510dbfda6:Services/church.backend/church.backend.services/Services/RolesServices.cs
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
