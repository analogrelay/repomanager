using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Security;
using Microsoft.AspNet.Security.DataProtection;
using Microsoft.AspNet.Security.Infrastructure;
using Microsoft.AspNet.Security.OAuth;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;
using System;
using System.Globalization;

namespace GitHubAuth
{
    public class GitHubAuthMiddleware : OAuthAuthenticationMiddleware<GitHubAuthOptions, IGitHubAuthNotifications>
    {
        public GitHubAuthMiddleware(
            RequestDelegate next,
            IServiceProvider services,
            IDataProtectionProvider dataProtectionProvider,
            ILoggerFactory loggerFactory,
            IOptions<ExternalAuthenticationOptions> externalOptions,
            IOptions<GitHubAuthOptions> options,
            ConfigureOptions<GitHubAuthOptions> configureOptions = null)
            : base(next, services, dataProtectionProvider, loggerFactory, externalOptions, options, configureOptions)
        {
            if (string.IsNullOrWhiteSpace(Options.ClientId))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "You must provide a client Id", "ClientId"));
            }
            if (string.IsNullOrWhiteSpace(Options.ClientSecret))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "You must provide a client secret", "ClientSecret"));
            }

            if (Options.Notifications == null)
            {
                Options.Notifications = new GitHubAuthNotifications();
            }

        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="GitHubOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<GitHubAuthOptions> CreateHandler()
        {
            return new GitHubAuthHandler(Backchannel, Logger);
        }
    }
}