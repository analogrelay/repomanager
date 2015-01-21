using Microsoft.AspNet.Http;
using Microsoft.AspNet.Loader.IIS;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace RepoManager.server.controllers
{
    public class ReposController : Controller
    {

        public ReposController()
        {
        }
        [Route("/api/repos")]
        public IActionResult GetAllRepositories()
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return new JsonResult("Must login");
            }

            var publicReposUrl = identity.Claims.First(x => x.Type == "github:repos:url").Value;

            HttpRequestMessage userRequest = new HttpRequestMessage(HttpMethod.Get, publicReposUrl);
			
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", identity.Claims.First(x=>x.Type== "github:accesstoken").Value);
            userRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            


            return new JsonResult(identity.Claims.First(x => x.Type == "github:accesstoken").Value);
        }
    }
}