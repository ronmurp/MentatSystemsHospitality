using Microsoft.AspNetCore.Mvc;
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
			return View("~/Views/Admin/Hotels/TestModelList.cshtml", new List<Hotel>());
		}
	}
	[HttpGet]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;

		//if (!string.IsNullOrEmpty(code))
		//{
		//	var testModels = hotelsRepoService.GetTestModelsAsync();
		//	var tm = testModels.FirstOrDefault(x => x.Code == code);
		//	if (tm != null)
		//	{
		//		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml", tm);
		//	}
		//}

		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
	}

	[HttpPost]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(TestModel testModel)
	{
		var testModels = await hotelsRepoService.GetTestModelsAsync();

		if (testModels.All(tm => tm.Code != testModel.Code))
		{
			testModels.Add(testModel);
			await hotelsRepoService.SaveTestModelsAsync(testModels);
			return RedirectToAction(nameof(TestModelAdd), new { IsSuccess = true });
		}

		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
	}
}