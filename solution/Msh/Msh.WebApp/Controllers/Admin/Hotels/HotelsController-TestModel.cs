using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Msh.Common.Exceptions;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models;

namespace Msh.WebApp.Controllers.Admin.Hotels;
public partial class HotelsController
{
	[Route("TestModelList")]
	public async Task<IActionResult> TestModelList()
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelsRepoService.GetTestModelsAsync();

			return View("~/Views/Admin/Hotels/TestModelList.cshtml", hotels);
		}
		catch (NullConfigException ex)
		{
			configRepository.SaveMissingConfig(ConstHotel.Cache.Hotel, new List<TestModel>());

			return View("~/Views/Admin/Hotels/TestModelList.cshtml", new List<TestModel>());
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View("~/Views/Admin/Hotels/TestModelList.cshtml", new List<TestModel>());
		}
	}

	[HttpGet]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();

		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
	}

	[HttpPost]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(TestModel testModel)
	{
		ViewBag.Languages = GetLanguages();

		if (ModelState.IsValid)
		{
			var testModels = await hotelsRepoService.GetTestModelsAsync();

			if (testModels.All(tm => tm.Code != testModel.Code))
			{
				testModels.Add(testModel);
				await hotelsRepoService.SaveTestModelsAsync(testModels);
				return RedirectToAction(nameof(TestModelAdd), new { IsSuccess = true, Code = testModel.Code });
			}
		}

		ViewBag.IsSuccess = false;
		ViewBag.Code = string.Empty;
		
		ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
	}

	private List<SelectListItem> GetLanguages() =>
	[
		new SelectListItem { Text = "English", Selected = true },
		new SelectListItem { Text = "French", Disabled = true },
		new SelectListItem { Text = "German" }
	];
}