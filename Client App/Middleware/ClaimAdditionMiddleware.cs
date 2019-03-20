using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DM.Service;

namespace ClientApp.Middleware
{
    public class ClaimAdditionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CidmService _cidmService;
  
        public ClaimAdditionMiddleware(RequestDelegate request)
        {
            _next = request;
            _cidmService = new CidmService();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userId = GetEmployeeId(httpContext.User);

            var roleId = _cidmService.GetRoleIdOfUser(userId);

            var appIdentity = httpContext.User.Identity as ClaimsIdentity;
            appIdentity?.AddClaim(new Claim("roleId", roleId));

            await _next(httpContext);
        }

        private int GetEmployeeId(ClaimsPrincipal principal)
        {
            if (!principal.HasClaim(claim => claim.Type == "EmployeeId"))
            {
                return new Random().Next();
            }

            var employeeId = principal.FindFirst(claim => claim.Type == "EmployeeId").Value;
            return Convert.ToInt32(employeeId);
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
