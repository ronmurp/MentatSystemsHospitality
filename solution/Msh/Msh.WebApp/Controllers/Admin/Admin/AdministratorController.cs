using Microsoft.AspNetCore.Mvc;
using Msh.WebApp.Services;

namespace Msh.WebApp.Controllers.Admin.Admin
{
	[Route("admin/administrator")]
	public class AdministratorController(IUserService userService) : Controller
	{

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Index()
		{
			await Task.Delay(0);
			return View("~/Views/Admin/Administrator/Index.cshtml");
		}

		[HttpGet]
		[Route("UserList")]
		public async Task<IActionResult> UserList()
		{
			await Task.Delay(0);

			var users = userService.GetUsers();

			return View("~/Views/Admin/Administrator/UserList.cshtml", users);
		}
	}
}
