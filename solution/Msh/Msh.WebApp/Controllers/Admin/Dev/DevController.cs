using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Controllers.Admin.Dev;

[Route("admin/dev")]
public class DevController(ILogger<HomeController> logger) : Controller
{
	private readonly ILogger<HomeController> _logger = logger;

	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);

		return View("~/Views/Admin/Dev/Index.cshtml");
	}

	[Route("ScriptsTest")]
	public async Task<IActionResult> ScriptsTest()
	{
		await Task.Delay(0);

		return View("~/Views/Admin/Dev/ScriptsTest.cshtml");
	}

}