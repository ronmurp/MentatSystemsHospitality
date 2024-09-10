using Microsoft.AspNetCore.Mvc;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[Controller]")]
	public class UsersController(IUserService userService) : Controller
	{
		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Index()
		{
			await Task.Delay(0);
			return View();
		}

		[HttpGet]
		[Route("UserList")]
		public async Task<IActionResult> UserList()
		{
			await Task.Delay(0);

			var users = userService.GetUsers();

			return View(users);
		}
	}
}
