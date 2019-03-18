using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Client_App
{
    public class RoleIdHandler : AuthorizationHandler<RoleIdRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleIdRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "RoleId"))
            {
                return Task.CompletedTask;
            }

            var roleId = Convert.ToInt32(context.User.FindFirst(c => c.Type == "RoleId"));

            if (requirement.RoleId == roleId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
