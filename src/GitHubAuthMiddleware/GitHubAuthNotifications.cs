using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Security.OAuth;

namespace GitHubAuth
{
    public class GitHubAuthNotifications : OAuthAuthenticationNotifications, IGitHubAuthNotifications
    {
        public GitHubAuthNotifications()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
        }

        public Func<GitHubAuthenticatedContext, Task> OnAuthenticated { get; set; }


        public Task Authenticated(GitHubAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }
    }
}