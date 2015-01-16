using Microsoft.AspNet.Http;
using Microsoft.AspNet.Security.OAuth;
using Newtonsoft.Json.Linq;
using System;

namespace GitHubAuth
{
    public class GitHubAuthenticatedContext : OAuthAuthenticatedContext
    {
        public GitHubAuthenticatedContext(HttpContext context, OAuthAuthenticationOptions options, JObject user, TokenResponse tokens)
            : base(context, options, user, tokens)
        {
            Login = TryGetValue(user, "login");
            Id = TryGetValue(user, "id");
            AvatarUrl = TryGetValue(user, "avatar_url");
            GravatarId = TryGetValue(user, "gravatar_id");
            Url = TryGetValue(user, "url");
            HTMLUrl = TryGetValue(user, "html_url");
            FollowersUrl = TryGetValue(user, "followers_url");
            FollowingUrl = TryGetValue(user, "following_url");
            GistsUrl = TryGetValue(user, "gists_url");
            StarredUrl = TryGetValue(user, "starred_url");
            SubscriptionUrl = TryGetValue(user, "subscriptions_url");
            OrganizationsUrl = TryGetValue(user, "organizations_url");
            ReposUrl = TryGetValue(user, "repos_url");
            EventsUrl = TryGetValue(user, "events_url");
            ReceivedEventsUrl = TryGetValue(user, "received_events_url");
            Type = TryGetValue(user, "type");
            SiteAdmin = bool.Parse(TryGetValue(user, "site_admin"));
            Name = TryGetValue(user, "name");
            Company = TryGetValue(user, "company");
            Blog = TryGetValue(user, "blog");
            Location = TryGetValue(user, "location");
            Email = TryGetValue(user, "email");
            Hireable = bool.Parse(TryGetValue(user, "hireable"));
            Bio = TryGetValue(user, "bio");
            PublicRepos = int.Parse(TryGetValue(user, "public_repos"));
            PublicGists = int.Parse(TryGetValue(user, "public_gists"));
            Followers = int.Parse(TryGetValue(user, "followers"));
            Following = int.Parse(TryGetValue(user, "following"));
            CreatedAt = DateTime.Parse(TryGetValue(user, "created_at"));
            UpdatedAt = DateTime.Parse(TryGetValue(user, "updated_at"));
        }

        public string Login { get; set; }
        public string Id { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarId { get; set; }
        public string Url { get; set; }
        public string HTMLUrl { get; set; }
        public string FollowersUrl { get; set; }
        public string FollowingUrl { get; set; }
        public string GistsUrl { get; set; }
        public string StarredUrl { get; set; }
        public string SubscriptionUrl { get; set; }
        public string OrganizationsUrl { get; set; }
        public string ReposUrl { get; set; }
        public string EventsUrl { get; set; }
        public string ReceivedEventsUrl { get; set; }
        public string Type { get; set; }
        public bool SiteAdmin { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Blog { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public bool Hireable { get; set; }
        public string Bio { get; set; }
        public int PublicRepos { get; set; }
        public int PublicGists { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private static string TryGetValue(JObject user, string propertyName)
        {
            JToken value;
            return user.TryGetValue(propertyName, out value) ? value.ToString() : null;
        }
    }

}