using Microsoft.AspNetCore.Mvc;

namespace Msh.WebApp.Areas.Admin.Controllers.Dev;

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

	[Route("ConfigStateList")]
	public async Task<IActionResult> ConfigStateList(bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			return View();
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("");
	}

}