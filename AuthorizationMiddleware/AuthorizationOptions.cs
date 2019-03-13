using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationMiddleware
{
    public class AuthorizationOptions : IAuthorizeData
    {
        public string Policy { get; set; }

        public string Roles { get; set; }

        public string AuthenticationSchemes { get; set; }
    }
}
