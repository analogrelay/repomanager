using Microsoft.AspNet.Http;
using Microsoft.AspNet.Loader.IIS;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RepoManager.server.controllers
{
    public class ReposController : Controller
    {
		HttpClient _httpClient;

		public ReposController()
        {
			_httpClient = new HttpClient();
		}

        [Route("/api/repos")]
        public async Task<IActionResult> GetAllRepositories()
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return new JsonResult("Must login");
            }

            var publicReposUrl = identity.Claims.First(x => x.Type == "github:repos:url").Value;
			var repoRequest = new HttpRequestMessage(HttpMethod.Get, publicReposUrl);
			repoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identity.Claims.First(x=>x.Type== "github:accesstoken").Value);
			repoRequest.Headers.Add("User-Agent", "TotallyAUserAgent"); //Without a user agent we get a malformed response from GitHub.
			repoRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			
			var repos = await _httpClient.SendAsync(repoRequest);
			var content = await repos.Content.ReadAsStringAsync();

            return new JsonResult(content);
        }
    }
}