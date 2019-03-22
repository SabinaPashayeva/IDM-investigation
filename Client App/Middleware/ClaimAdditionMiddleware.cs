using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Client_App.Services;
using Microsoft.AspNetCore.Http;
using DM.Service;

namespace ClientApp.Middleware
{
    public class ClaimAdditionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICidmService _cidmService;
        private readonly IAppMemoryCache _memoryCache;
  
        public ClaimAdditionMiddleware(RequestDelegate request, 
            ICidmService cidmService,
            IAppMemoryCache memoryCache)
        {
            _next = request;
            _cidmService = cidmService;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
            {
                await _next(httpContext);
                return;
            }

            var userId = GetEmployeeId(httpContext.User);
            
            var appIdentity = httpContext.User.Identity as ClaimsIdentity;
            appIdentity?.AddClaim(new Claim("roleId", GetRoleId(userId)));

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

        private string GetRoleId(int userId)
        {
            var roleId = _memoryCache.TryGetValue<string>();
            if (string.IsNullOrWhiteSpace(roleId))
            {
                roleId = _cidmService.GetRoleIdOfUser(userId);
                _memoryCache.SetValue(roleId);
            }

            return roleId;
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
