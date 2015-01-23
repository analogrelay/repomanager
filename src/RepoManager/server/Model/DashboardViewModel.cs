using System;

namespace RepoManager.Server.Model
{
    public class DashboardViewModel
    {
        public string Name { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public int PublicGists { get; set; }
        public int PublicRepos { get; set; }
        public Uri FollowersUri {get;set;}
        public Uri FollowingUri { get;set;}
        public Uri PublicGistsUri { get;set; }
        public Uri PublicReposUri { get;set; }
        public Uri AvatarUri { get; set; }
    }
}