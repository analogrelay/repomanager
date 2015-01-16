using System.Net.Http;
using Microsoft.Framework.Logging;
using Microsoft.AspNet.Security.OAuth;
using Microsoft.AspNet.Http.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Security;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace GitHubAuth
{
    public class GitHubAuthHandler :  OAuthAuthenticationHandler<GitHubAuthOptions, IGitHubAuthNotifications>
    {
        private HttpClient backchannel;
        private ILogger logger;

        public GitHubAuthHandler(HttpClient backchannel, ILogger logger) : base(backchannel, logger)
        {
            this.backchannel = backchannel;
            this.logger = logger;
        }

        protected override async Task<AuthenticationTicket> GetUserInformationAsync(AuthenticationProperties properties, TokenResponse tokens)
        {
            // Get the GitHub user
            HttpRequestMessage userRequest = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
            userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage userResponse = await Backchannel.SendAsync(userRequest, Context.RequestAborted);
            userResponse.EnsureSuccessStatusCode();
            var text = await userResponse.Content.ReadAsStringAsync();
            JObject user = JObject.Parse(text);

            var context = new GitHubAuthenticatedContext(Context, Options, user, tokens);

            context.Identity = new ClaimsIdentity(
                Options.AuthenticationType,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            //TODO: Theoretically I could generate this code with a preprocessor. Write something nice here and generate the rest.
            //Effectively the same as using reflection but at build time.
            if (!string.IsNullOrEmpty(context.Login))
            {
                context.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, context.Login, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Id))
            {
                context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.Id, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.AvatarUrl))
            {
                context.Identity.AddClaim(new Claim("github:avatar:url", context.AvatarUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.GravatarId))
            {
                context.Identity.AddClaim(new Claim("github:avatar:url", context.GravatarId, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Url))
            {
                context.Identity.AddClaim(new Claim("github:url", context.Url, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.HTMLUrl))
            {
                context.Identity.AddClaim(new Claim("github:html:url", context.HTMLUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.FollowersUrl))
            {
                context.Identity.AddClaim(new Claim("github:followers:url", context.FollowersUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.FollowingUrl))
            {
                context.Identity.AddClaim(new Claim("github:following:url", context.FollowingUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.GistsUrl))
            {
                context.Identity.AddClaim(new Claim("github:gists:url", context.GistsUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.StarredUrl))
            {
                context.Identity.AddClaim(new Claim("github:starred:url", context.StarredUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.SubscriptionUrl))
            {
                context.Identity.AddClaim(new Claim("github:subscriptions:url", context.SubscriptionUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.OrganizationsUrl))
            {
                context.Identity.AddClaim(new Claim("github:organizations:url", context.OrganizationsUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.ReposUrl))
            {
                context.Identity.AddClaim(new Claim("github:repos:url", context.ReposUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.EventsUrl))
            {
                context.Identity.AddClaim(new Claim("github:events:url", context.EventsUrl, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.ReceivedEventsUrl))
            {
                context.Identity.AddClaim(new Claim("github:received:events:url", context.Url, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Type))
            {
                context.Identity.AddClaim(new Claim("github:type:url", context.Type, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            
            context.Identity.AddClaim(new Claim("github:site:admin", context.SiteAdmin.ToString(), ClaimValueTypes.Boolean, context.Options.AuthenticationType));
         
            if (!string.IsNullOrEmpty(context.Name))
            {
                context.Identity.AddClaim(new Claim("github:name", context.Name, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Company))
            {
                context.Identity.AddClaim(new Claim("github:company", context.Company, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Blog))
            {
                context.Identity.AddClaim(new Claim("github:blog", context.Blog, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Location))
            {
                context.Identity.AddClaim(new Claim("github:location", context.Location, ClaimValueTypes.String, context.Options.AuthenticationType));
            }
            if (!string.IsNullOrEmpty(context.Email))
            {
                context.Identity.AddClaim(new Claim("github:email", context.Email, ClaimValueTypes.String, context.Options.AuthenticationType));
            }

            context.Identity.AddClaim(new Claim("github:hireable", context.Hireable.ToString(), ClaimValueTypes.Boolean, context.Options.AuthenticationType));
          
            if (!string.IsNullOrEmpty(context.Bio))
            {
                context.Identity.AddClaim(new Claim("github:bio", context.Bio, ClaimValueTypes.String, context.Options.AuthenticationType));
            }

            context.Identity.AddClaim(new Claim("github:public:repos", context.PublicRepos.ToString(), ClaimValueTypes.Integer, context.Options.AuthenticationType));
            context.Identity.AddClaim(new Claim("github:public:gists", context.PublicGists.ToString(), ClaimValueTypes.Integer, context.Options.AuthenticationType));
            context.Identity.AddClaim(new Claim("github:followers", context.Followers.ToString(), ClaimValueTypes.Integer, context.Options.AuthenticationType));
            context.Identity.AddClaim(new Claim("github:following", context.Following.ToString(), ClaimValueTypes.Integer, context.Options.AuthenticationType));

            context.Identity.AddClaim(new Claim("github:created", context.CreatedAt.ToString(), ClaimValueTypes.DateTime, context.Options.AuthenticationType));
            context.Identity.AddClaim(new Claim("github:updated", context.UpdatedAt.ToString(), ClaimValueTypes.DateTime, context.Options.AuthenticationType));

            context.Properties = properties;

            await Options.Notifications.Authenticated(context);

            return new AuthenticationTicket(context.Identity, context.Properties);

        }

    }
}