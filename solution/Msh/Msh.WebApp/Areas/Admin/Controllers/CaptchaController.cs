using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.Captcha;
using Msh.Common.Services;

namespace Msh.WebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Route("admin/captcha")]
public class CaptchaController(
	ILogger<WebApp.Controllers.HomeController> logger,
	ICaptchaConfigRepoService captchaConfigRepoService)
	: Controller
{
	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);

		return View();
	}

	[Route("Config")]
	public async Task<IActionResult> Config()
	{
		try
		{
			await Task.Delay(0);

			var config = await captchaConfigRepoService.GetData();

			return View(config);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View(new CaptchaConfig());
		}
	}

	[HttpPost]
	[Route("Config")]
	public async Task<IActionResult> Config([FromForm] CaptchaConfig config, string command)
	{
		try
		{
			await Task.Delay(0);

			if (command == "Save")
			{
				await captchaConfigRepoService.Save(config);
			}
			else if (command == "Publish")
			{
				//captchaConfigRepoService.Re();
			}

			return RedirectToAction("Config");
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return View(config);

	}


}