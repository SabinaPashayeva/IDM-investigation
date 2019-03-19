using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ClientApp.Middleware
{
    public class ClaimAdditionMiddleware
    {
        private readonly RequestDelegate _next;

        public ClaimAdditionMiddleware(RequestDelegate request)
        {
            _next = request;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userId = httpContext.User.FindFirst(c => c.Type == "EmployeeId");
            var applicationId = "some value";

            //send request to CIDM
            //get response

            //example of output
            var roleModel = new RoleModel
            {
                RoleId = 143
            };

            var appIdentity = httpContext.User.Identity as ClaimsIdentity;
            appIdentity.AddClaim(new Claim("roleId", roleModel.RoleId.ToString()));

            await _next(httpContext);
        }
    }

    internal class RoleModel
    {
        public int RoleId { get; set; }
        public int ResourceTypeId { get; set; }
        public string RoleDescription { get; set; }
        public int ApplicationId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
