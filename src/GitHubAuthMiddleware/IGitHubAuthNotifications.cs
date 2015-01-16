using Microsoft.AspNet.Security.OAuth;
using System.Threading.Tasks;

namespace GitHubAuth
{
    public interface IGitHubAuthNotifications : IOAuthAuthenticationNotifications
    {
        /// <summary>
        /// Invoked whenever GitHub succesfully authenticates a user.
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        Task Authenticated(GitHubAuthenticatedContext context);
    }
}