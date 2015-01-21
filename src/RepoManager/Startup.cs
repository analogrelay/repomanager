using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Security;
using Microsoft.AspNet.Security.Cookies;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using Microsoft.AspNet.Http.Security;
using Microsoft.AspNet.Security.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.Framework.Runtime;

namespace RepoManager
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup()
        {
            Configuration = new Configuration()
                .AddIniFile("configuration.ini")
                .AddEnvironmentVariables();
        }
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
			
            loggerFactory.AddConsole();
            app.UseErrorPage();

            app.UseStaticFiles();

            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new PathString("/login");
            });

            app.UseGitHubAuthentication(options =>
            {
                options.ClientId = Configuration.Get("GITHUB:CLIENT_ID");
                options.ClientSecret = Configuration.Get("GITHUB:CLIENT_SECRET");
            });

            app.Map("/login", signoutApp =>
            {
                signoutApp.Run(async context =>
                {
                    context.Response.Challenge(new AuthenticationProperties() { RedirectUri = "/" }, "GitHub");
                    return;
                });
            });

            app.Map("/logout", signoutApp =>
            {
                signoutApp.Run(async context =>
                {
                    context.Response.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                    context.Response.Redirect("/");
                });
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.Contains("api"))
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        context.Response.Challenge(new AuthenticationProperties() { RedirectUri = context.Request.Path.ToString() }, "GitHub");
                        return;
                    }
                }
                await next();
            });

            app.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ExternalAuthenticationOptions>(options =>
            {
                options.SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType;
            });
            
            services.AddMvc();
        }
    }
}
