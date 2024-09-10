using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/dev")]
public class DevController(ILogger<WebApp.Controllers.HomeController> logger) : Controller
{
	private readonly ILogger<WebApp.Controllers.HomeController> _logger = logger;

	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);

		return View();
	}

	[Route("ScriptsTest")]
	public async Task<IActionResult> ScriptsTest()
	{
		await Task.Delay(0);

		return View();
	}

}