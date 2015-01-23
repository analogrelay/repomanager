using GitHubAuth;
using Microsoft.AspNet.Mvc;
using RepoManager.Server.Model;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RepoManager.Server.controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("~/server/Views/Home/Index.cshtml");
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            var dashboardData = new DashboardViewModel {
                AvatarUri = new Uri(Context.User.FindFirst(ClaimKeys.AVATAR_URL).Value),
                Followers = int.Parse(Context.User.FindFirst(ClaimKeys.FOLLOWERS).Value),
                FollowersUri = new Uri(Context.User.FindFirst(ClaimKeys.FOLLOWERS_URL).Value),
                Following = int.Parse(Context.User.FindFirst(ClaimKeys.FOLLOWING).Value),
                FollowingUri = new Uri(Context.User.FindFirst(ClaimKeys.FOLLOWING_URL).Value),
                Name = Context.User.Identity.Name,
                PublicGists = int.Parse(Context.User.FindFirst(ClaimKeys.PUBLIC_GISTS).Value),
                PublicGistsUri = new Uri(Context.User.FindFirst(ClaimKeys.PUBLIC_GISTS_URL).Value),
                PublicRepos = int.Parse(Context.User.FindFirst(ClaimKeys.REPOS).Value),
                PublicReposUri = new Uri(Context.User.FindFirst(ClaimKeys.REPOS_URL).Value)
            };

            return View("~/server/Views/Home/Dashboard.cshtml", dashboardData);
        }

        [Authorize]
        public IActionResult Issues()
		{
			return View("~/server/Views/Home/Issues.cshtml");
		}

        [Authorize]
        public IActionResult Settings()
        {
            return View("~/server/Views/Home/Settings.cshtml");
        }

        [Authorize]
        public IActionResult Users()
        {
            return View("~/server/Views/Home/Dashboard.cshtml");
        }
    }
}
