using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Controllers.Admin
{
	[Route("admin")]
	public class AdminController : Controller
	{
		[Route("")]
		public IActionResult Index()
		{
			return View("~/Views/Admin/Index.cshtml");
		}
	}
}
