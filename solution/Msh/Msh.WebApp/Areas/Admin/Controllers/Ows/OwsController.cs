﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.HotelCache.Models;
using Msh.Opera.Ows.Cache;
using Msh.Opera.Ows.Models;


namespace Msh.WebApp.Areas.Admin.Controllers.Ows;

[Authorize]
[Area("Admin")]
[Route("admin/ows")]
public class OwsController(ILogger<OwsController> logger,
	IOwsRepoService owsRepoService) : Controller
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



		var owsConfig = await owsRepoService.GetOwsConfigAsync();
		if (owsConfig == null)
		{
			owsConfig = new OwsConfig();
			await owsRepoService.SaveOwsConfigAsync(owsConfig);
		}

		return View(owsConfig);

	}

	[HttpPost]
	[Route("OwsConfigEdit")]
	public async Task<IActionResult> OwsConfigEdit([FromForm] OwsConfig owsConfig)
	{
		
		if (ModelState.IsValid)
		{

			var owsConfigCurrent = await owsRepoService.GetOwsConfigAsync();

			// Retain properties not edited here
			owsConfig.CriticalErrorTriggers = owsConfigCurrent.CriticalErrorTriggers;
			owsConfig.SchemeMap = owsConfigCurrent.SchemeMap;

			await owsRepoService.SaveOwsConfigAsync(owsConfig);

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

	
	[Route("OwsConfigEditMap")]
	public async Task<IActionResult> OwsConfigEditMap(bool isSuccess = false)
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

		return RedirectToAction("Index");
	}

	[Route("OwsConfigEditTriggers")]
	public async Task<IActionResult> OwsConfigEditTriggers(bool isSuccess = false)
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

		return RedirectToAction("Index");
	}

	[Route("OwsRawAvailability")]
	public async Task<IActionResult> OwsRawAvailability(bool isSuccess = false)
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

		return RedirectToAction("Index");
	}

}