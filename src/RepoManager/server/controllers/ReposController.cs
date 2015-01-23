using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace RepoManager.Server.controllers
{
	[Authorize]
	public class ReposController : Controller
    {
		HttpClient _httpClient;
		ClaimsIdentity _identity;
		Uri _gitHubApiUrl;

		public ReposController()
        {
			_httpClient = new HttpClient();
			_gitHubApiUrl = new Uri("https://api.github.com");
		}

		[Route("/api/repos")]
        public async Task<IActionResult> GetAllRepositories()
        {
			var reposUrl = new Uri(Context.User.Claims.First(x => x.Type == "github:repos:url").Value);
			var repos = await _httpClient.SendAsync(BuildRequestMessage(HttpMethod.Get, reposUrl));
			var content = await repos.Content.ReadAsStringAsync();
			
            return new JsonResult(content);
        }

		[Route("/api/repos/{organisation}")]
		public async Task<IActionResult> GetRepositoriesForOrganisation(string organisation)
		{
			var orgReposUrl = new Uri(_gitHubApiUrl, "/orgs/" + organisation + "/repos");
			var repos = await _httpClient.SendAsync(BuildRequestMessage(HttpMethod.Get, orgReposUrl));
			var content = await repos.Content.ReadAsStringAsync();

			return new JsonResult(content);
		}

		[Route("/api/orgs")]
		public async Task<IActionResult> GetOrganisation()
		{
			var orgsUrl = new Uri(_gitHubApiUrl, "user/orgs");
			var repos = await _httpClient.SendAsync(BuildRequestMessage(HttpMethod.Get, orgsUrl));
			var content = await repos.Content.ReadAsStringAsync();

			return new JsonResult(content);
		}

		private HttpRequestMessage BuildRequestMessage(HttpMethod method, Uri uri)
		{
			var requestMessage = new HttpRequestMessage(method, uri);
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Context.User.Claims.First(x => x.Type == "github:accesstoken").Value);
			requestMessage.Headers.Add("User-Agent", "RepoManager"); //Without a user agent we get a malformed response from GitHub.
			requestMessage.Headers.Add("Accept", "application/vnd.github.moondragon+json");

			return requestMessage;

		}

	}
}