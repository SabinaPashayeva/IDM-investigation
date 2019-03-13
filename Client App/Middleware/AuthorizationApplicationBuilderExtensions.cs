using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace Client_App.Middleware
{
    public static class AuthorizationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app, string policyName)
        {
            return app.UseMiddleware(typeof(AuthorizationMiddleware), policyName);
        }
    }
}
