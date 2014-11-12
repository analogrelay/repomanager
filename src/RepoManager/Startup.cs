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
            app.UseErrorHandler("/home/error");

            app.UseStaticFiles();

            app.UseCookieAuthentication(options =>
            {
                options.LoginPath = new PathString("/login");
            });

            app.UseOAuthAuthentication("GitHub-AccessToken", options =>
            {
                options.ClientId = Configuration.Get("GITHUB:CLIENT_ID");
                options.ClientSecret = Configuration.Get("GITHUB:CLIENT_SECRET");
                options.CallbackPath = new PathString("/signin");
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.UserInformationEndpoint = "https://api.github.com/user";

                options.Notifications = new OAuthAuthenticationNotifications()
                {
                    OnGetUserInformationAsync = async (context) =>
                    {
                        // Get the GitHub user
                        HttpRequestMessage userRequest = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage userResponse = await context.Backchannel.SendAsync(userRequest, context.HttpContext.RequestAborted);
                        userResponse.EnsureSuccessStatusCode();
                        var text = await userResponse.Content.ReadAsStringAsync();
                        JObject user = JObject.Parse(text);

                        var identity = new ClaimsIdentity(
                            context.Options.AuthenticationType,
                            ClaimsIdentity.DefaultNameClaimType,
                            ClaimsIdentity.DefaultRoleClaimType);

                        JToken value;
                        var id = user.TryGetValue("id", out value) ? value.ToString() : null;
                        if (!string.IsNullOrEmpty(id))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id, ClaimValueTypes.String, context.Options.AuthenticationType));
                        }
                        var userName = user.TryGetValue("login", out value) ? value.ToString() : null;
                        if (!string.IsNullOrEmpty(userName))
                        {
                            identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, userName, ClaimValueTypes.String, context.Options.AuthenticationType));
                        }
                        var name = user.TryGetValue("name", out value) ? value.ToString() : null;
                        if (!string.IsNullOrEmpty(name))
                        {
                            identity.AddClaim(new Claim("urn:github:name", name, ClaimValueTypes.String, context.Options.AuthenticationType));
                        }
                        var link = user.TryGetValue("url", out value) ? value.ToString() : null;
                        if (!string.IsNullOrEmpty(link))
                        {
                            identity.AddClaim(new Claim("urn:github:url", link, ClaimValueTypes.String, context.Options.AuthenticationType));
                        }
                        var avatar = user.TryGetValue("avatar_url", out value) ? value.ToString() : null;
                        if (!string.IsNullOrEmpty(avatar))
                        {
                            identity.AddClaim(new Claim("urn:github:avatar:url", avatar, ClaimValueTypes.String, context.Options.AuthenticationType));
                        }
                        context.Identity = identity;
                    },
                };
            });

            // Choose an authentication type
            app.Map("/login", signoutApp =>
            {
                signoutApp.Run(async context =>
                {
                    context.Response.Challenge(new AuthenticationProperties() { RedirectUri = "/" }, "GitHub-AccessToken");
                    return;
                });
            });

            // Sign-out to remove the user cookie.
            app.Map("/logout", signoutApp =>
            {
                signoutApp.Run(async context =>
                {
                    context.Response.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                    context.Response.Redirect("/");
                });
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
