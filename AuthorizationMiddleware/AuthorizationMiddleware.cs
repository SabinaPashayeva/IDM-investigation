using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthorizationMiddleware
{ 
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthorizeData[] _authorizeData;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private AuthorizationPolicy _authorizationPolicy;

        public AuthorizationMiddleware(
            RequestDelegate next,
            IAuthorizationPolicyProvider policyProvider,
            IOptions<AuthorizationOptions> authorizationOptions)
        {
            _next = next;
            _authorizeData = new[] { authorizationOptions.Value };
            _policyProvider = policyProvider;
        }

        public async Task Invoke(HttpContext httpContext, IPolicyEvaluator policyEvaluator)
        {
            if (_authorizationPolicy is null)
            {
                _authorizationPolicy =
                    await AuthorizationPolicy.CombineAsync(_policyProvider, _authorizeData);
            }

            var authenticateResult =
                await policyEvaluator.AuthenticateAsync(_authorizationPolicy, httpContext);
            var authorizeResult =
                await policyEvaluator.AuthorizeAsync(_authorizationPolicy, authenticateResult, httpContext, null);

            if (authorizeResult.Challenged)
            {
                await ChallengeAsync(httpContext);
                return;
            }
            else if (authorizeResult.Forbidden)
            {
                await ForbidAsync(httpContext);
                return;
            }

            await _next(httpContext);
        }

        private async Task ChallengeAsync(HttpContext httpContext)
        {
            if (_authorizationPolicy.AuthenticationSchemes.Count > 0)
            {
                foreach (string authenticationScheme in _authorizationPolicy.AuthenticationSchemes)
                {
                    await httpContext.ChallengeAsync(authenticationScheme);
                }
            }
            else
            {
                await httpContext.ChallengeAsync();
            }
        }

        private async Task ForbidAsync(HttpContext httpContext)
        {
            if (_authorizationPolicy.AuthenticationSchemes.Count > 0)
            {
                foreach (string authenticationScheme in _authorizationPolicy.AuthenticationSchemes)
                {
                    await httpContext.ForbidAsync(authenticationScheme);
                }
            }
            else
            {
                await httpContext.ForbidAsync();
            }
        }
    }
}
