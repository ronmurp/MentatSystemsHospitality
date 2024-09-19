using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models;
using Msh.HotelCache.Services;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Services.Config;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
[Route("admin/ows")]
public class OwsController(ILogger<OwsController> logger,
	IOwsConfigService owsConfigService) : Controller
{

	[HttpGet]
	[Route("")]
	public async Task<IActionResult> Index()
	{
		return View();
	}


	[HttpGet]
	[Route("OwsConfigEdit")]
	public async Task<IActionResult> OwsConfigEdit(bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;



		var owsConfig = await owsConfigService.GetOwsConfigAsync();
		if (owsConfig == null)
		{
			owsConfig = new OwsConfig();
			await owsConfigService.SaveHotelsAsync(owsConfig);
		}

		return View(owsConfig);

	}

	[HttpPost]
	[Route("OwsConfigEdit")]
	public async Task<IActionResult> OwsConfigEdit([FromForm] OwsConfig owsConfig)
	{
		
		if (ModelState.IsValid)
		{

			var owsConfigCurrent = await owsConfigService.GetOwsConfigAsync();

			// Retain properties not edited here
			owsConfig.CriticalErrorTriggers = owsConfigCurrent.CriticalErrorTriggers;
			owsConfig.SchemeMap = owsConfigCurrent.SchemeMap;

			await owsConfigService.SaveHotelsAsync(owsConfig);

			return RedirectToAction(nameof(OwsConfigEdit), new { IsSuccess = true });

		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View();
		}
	}
}