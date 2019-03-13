using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace AuthorizationMiddleware
{
    public static class AuthorizationApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app,
            AuthorizationOptions authorizationOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (authorizationOptions == null)
            {
                throw new ArgumentNullException(nameof(authorizationOptions));
            }

            return app.UseMiddleware(typeof(AuthorizationMiddleware), Options.Create(authorizationOptions));
        }
    }
}
