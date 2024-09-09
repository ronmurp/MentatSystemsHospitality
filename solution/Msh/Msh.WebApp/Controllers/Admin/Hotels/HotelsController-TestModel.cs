using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Msh.Common.Exceptions;
using Msh.Common.Models.ViewModels;
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
		ViewBag.Hotels = await GetHotels();

		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
	}

	[HttpPost]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(TestModel testModel)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var testModels = await hotelsRepoService.GetTestModelsAsync();

			if (testModels.All(tm => tm.Code != testModel.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				testModel.Notes = string.IsNullOrEmpty(testModel.Notes) ? string.Empty : testModel.Notes;

				testModels.Add(testModel);
				await hotelsRepoService.SaveTestModelsAsync(testModels);
				return RedirectToAction(nameof(TestModelAdd), new { IsSuccess = true, Code = testModel.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
		}

		
	}


	[HttpGet]
	[Route("TestModelEdit/{code}")]
	public async Task<IActionResult> TestModelEdit(string code, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		var testModels = await hotelsRepoService.GetTestModelsAsync();
		var testModel = testModels.FirstOrDefault(m => m.Code == code);
		if (testModel != null)
		{
			return View("~/Views/Admin/Hotels/TestModelEdit.cshtml", testModel);
		}

		return RedirectToAction(nameof(TestModelList));
	}


	[HttpPost]
	[Route("TestModelEdit")]
	public async Task<IActionResult> TestModelEdit(TestModel testModel)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var testModels = await hotelsRepoService.GetTestModelsAsync();
			var index = testModels.FindIndex(m => m.Code == testModel.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				testModel.Notes = string.IsNullOrEmpty(testModel.Notes) ? string.Empty : testModel.Notes;

				testModels[index] = testModel;
				await hotelsRepoService.SaveTestModelsAsync(testModels);
				return RedirectToAction(nameof(TestModelEdit), new { IsSuccess = true, Code = testModel.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code does not exist");

				return View("~/Views/Admin/Hotels/TestModelEdit.cshtml");
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View("~/Views/Admin/Hotels/TestModelEdit.cshtml");
		}

	}




	[HttpPost]
	[Route("TestModelDelete")]
	public async Task<IActionResult> TestModelDelete([FromQuery] string code)
	{
		try
		{
			var testModels = await hotelsRepoService.GetTestModelsAsync();
			if (testModels.Any(m => m.Code == code))
			{
				var tm = testModels.First(m => m.Code == code);
				testModels.Remove(tm);
				await hotelsRepoService.SaveTestModelsAsync(testModels);

				return Ok(new ObjectVm());
			}
			else
			{
				return RedirectToAction(nameof(TestModelList));
			}

			
		}
		catch (Exception ex)
		{
			return RedirectToAction(nameof(TestModelList));
		}

		
		//return View("~/Views/Admin/Hotels/TestModelList.cshtml");
	}

	private List<SelectListItem> GetLanguages()
	{
		var gEurope = new SelectListGroup { Name = "Europe" };
		var gAsia = new SelectListGroup { Name = "Asia" };
		return [
			new SelectListItem { Text = "English", Selected = true, Group = gEurope},
			new SelectListItem { Text = "French", Disabled = true, Group = gEurope },
			new SelectListItem { Text = "German", Group = gEurope },
			new SelectListItem { Text = "Chinese", Group = gAsia },
			new SelectListItem { Text = "Japanese", Group = gAsia }
		];
	}

	private async Task<List<SelectListItem>> GetHotels()
	{
		var hotels = await hotelsRepoService.GetHotelsAsync();

		var list = new List<SelectListItem>();

		foreach (var h in hotels)
		{
			list.Add(new SelectListItem { Value = h.HotelCode, Text = h.Name });
		}

		return list;
	}
}