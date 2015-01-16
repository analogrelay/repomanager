using GitHubAuth;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.OptionsModel;
using System;

namespace Microsoft.AspNet.Builder
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGitHubAuthentication(this IApplicationBuilder app, Action<GitHubAuthOptions> configureOptions = null, string optionsName = "")
        {
            return app.UseMiddleware<GitHubAuthMiddleware>(
                 new ConfigureOptions<GitHubAuthOptions>(configureOptions ?? (o => { }))
                 {
                     Name = optionsName
                 });
        }
    }
}