using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RepoManager.server.controllers
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
            return View("~/server/Views/Home/Dashboard.cshtml");
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
