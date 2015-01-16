using Microsoft.AspNet.Http;
using Microsoft.AspNet.Security.OAuth;

namespace GitHubAuth
{
    public class GitHubAuthOptions : OAuthAuthenticationOptions<IGitHubAuthNotifications>
    {
        public GitHubAuthOptions()
        {
            AuthenticationType = GitHubAuthDefaults.AuthenticationType;
            Caption = AuthenticationType;
            CallbackPath = new PathString("/signin-github");
            AuthorizationEndpoint = GitHubAuthDefaults.AuthorizationEndpoint;
            TokenEndpoint = GitHubAuthDefaults.TokenEndpoint;
            UserInformationEndpoint = GitHubAuthDefaults.UserInformationEndpoint;
        }

    }
}