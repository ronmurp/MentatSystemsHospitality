using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.HotelCache.Models;
using Msh.Loggers.XmlLogger;

namespace Msh.WebApp.Areas.Admin.Controllers.Loggers;

[Authorize]
[Area("Admin")]
[Route("admin/loggers")]
public class LoggersController(ILogger<LoggersController> logger,
	ILogXmlRepoService logXmlRepoService) : Controller
{

	[HttpGet]
	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);
		return View();
	}


	[HttpGet]
	[Route("LogXmlConfigEdit/{group}")]
	public async Task<IActionResult> LogXmlConfigEdit(string group, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Group = group;

		var logXmlConfig = await logXmlRepoService.GetConfig(group);
		if (logXmlConfig == null)
		{
			logXmlConfig = new LogXmlConfig();
			await logXmlRepoService.SaveConfig(logXmlConfig, group);
		}

		return View(logXmlConfig);

	}

	[HttpPost]
	[Route("LogXmlConfigEdit/{group}")]
	public async Task<IActionResult> LogXmlConfigEdit([FromForm] LogXmlConfig logXmlConfig, string group)
	{
		
		if (ModelState.IsValid)
		{

			var logXmlConfigCurrent = await logXmlRepoService.GetConfig(group);

			// Retain properties not edited here
			logXmlConfig.Items = logXmlConfigCurrent.Items;

			await logXmlRepoService.SaveConfig(logXmlConfig, group);

			return RedirectToAction(nameof(LogXmlConfigEdit), new { Group = group, IsSuccess = true });

		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View();
		}
	}


	[Route("LogXmlConfigEditItems/{group}")]
	public async Task<IActionResult> LogXmlConfigEditItems(string group, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.Group = group;

			return View();
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("Index");
	}

	//[Route("OwsConfigEditTriggers")]
	//public async Task<IActionResult> OwsConfigEditTriggers(bool isSuccess = false)
	//{
	//	try
	//	{
	//		await Task.Delay(0);

	//		return View();
	//	}
	//	catch (Exception ex)
	//	{
	//		logger.LogError($"{ex.Message}");
	//	}

	//	return RedirectToAction("Index");
	//}

	//[Route("OwsRawAvailability")]
	//public async Task<IActionResult> OwsRawAvailability(bool isSuccess = false)
	//{
	//	try
	//	{
	//		await Task.Delay(0);

	//		return View();
	//	}
	//	catch (Exception ex)
	//	{
	//		logger.LogError($"{ex.Message}");
	//	}

	//	return RedirectToAction("Index");
	//}

}