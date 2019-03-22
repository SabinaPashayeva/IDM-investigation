using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Client_App
{
    public class RoleIdRequirement : IAuthorizationRequirement
    {
        public int RoleId { get; }

        public RoleIdRequirement(int id) => RoleId = id;
    }
}
