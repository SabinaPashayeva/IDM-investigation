using System;
using Microsoft.AspNetCore.Builder;

namespace ClientApp.Middleware
{
    public static class ClaimAdditionApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseClaimAddition(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware(typeof(ClaimAdditionMiddleware));
        }
    }
}
