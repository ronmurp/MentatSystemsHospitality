using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin")]
	public class HomeController : Controller
	{
		[Route("")]
		public ActionResult Index()
		{
			return View();
		}

	}
}
