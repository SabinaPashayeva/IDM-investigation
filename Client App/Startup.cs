using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClientApp.Middleware;
using Client_App.Services;
using DM.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Client_App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // turned off the JWT claim type mapping to allow well-known claims(e.g. ‘sub’ and ‘idp’) to flow through unmolested:
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    // using a cookie to locally sign-in the user 
                    options.DefaultScheme = "Cookies";
                    // using the OpenID Connect protocol
                    options.DefaultChallengeScheme = "oidc";
                })
                // add the handler that can process cookies
                .AddCookie("Cookies")
                //configure the handler that perform the OpenID Connect protocol
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "http://localhost:53325";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.SaveTokens = true;
                });

            services.AddAuthorization(options =>
                    {
                        options.AddPolicy("DeveloperToolPolicy", policy => policy.Requirements.Add(new RoleIdRequirement(143))); //id from config file                                                                                    
                        options.AddPolicy("CustomerOnboardingPolicy", policy => policy.Requirements.Add(new RoleIdRequirement(147)));
                    })
                .AddAuthorizationPolicyEvaluator();

            services.AddMemoryCache();
            services.AddSingleton<IAppMemoryCache, AppMemoryCache>();
            services.AddSingleton<IAuthorizationHandler, RoleIdHandler>();
            services.AddTransient<ICidmService, CidmService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseClaimAddition();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
