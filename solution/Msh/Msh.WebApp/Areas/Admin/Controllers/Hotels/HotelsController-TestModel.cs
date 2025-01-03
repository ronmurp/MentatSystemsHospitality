﻿using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;
public partial class HotelsController
{

	[Route("TestModelList")]
	public async Task<IActionResult> TestModelList()
	{
		try
		{
			var hotels = await testModelRepository.GetData();

			return View(hotels);
		}
		catch (NullConfigException ex)
		{
			//await specialsRepository.SaveMissingConfigAsync(ConstHotel.Cache.Hotel, new List<TestModel>());

			return View(new List<TestModel>());
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View(new List<TestModel>());
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

		return View();
	}


	[HttpPost]
	[Route("TestModelAdd")]
	public async Task<IActionResult> TestModelAdd(TestModel testModel)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var testModels = await testModelRepository.GetData();

			if (testModels.All(tm => tm.Code != testModel.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				testModel.Notes = string.IsNullOrEmpty(testModel.Notes) ? string.Empty : testModel.Notes;

				testModels.Add(testModel);
				await testModelRepository.Save(testModels);
				return RedirectToAction(nameof(TestModelAdd), new { IsSuccess = true, Code = testModel.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View();
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View();
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

		var testModels = await testModelRepository.GetData();
		var testModel = testModels.FirstOrDefault(m => m.Code == code);
		if (testModel != null)
		{
			return View(testModel);
		}

		return RedirectToAction(nameof(TestModelList));
	}


	[HttpPost]
	[Route("TestModelEdit/{hotelCode}")]
	public async Task<IActionResult> TestModelEdit(TestModel testModel, string hotelCode = "")
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var testModels = await testModelRepository.GetData();
			var index = testModels.FindIndex(m => m.Code == testModel.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				testModel.Notes = string.IsNullOrEmpty(testModel.Notes) ? string.Empty : testModel.Notes;

				testModels[index] = testModel;
				await testModelRepository.Save(testModels);
				return RedirectToAction(nameof(TestModelEdit), new { IsSuccess = true, Code = testModel.Code });
				
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code does not exist");

				return View();
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View();
		}
	}



	[HttpPost]
	[Route("TestModelDelete")]
	public async Task<IActionResult> TestModelDelete([FromQuery] string code)
	{
		try
		{
			var testModels = await testModelRepository.GetData();
			if (testModels.Any(m => m.Code == code))
			{
				var tm = testModels.First(m => m.Code == code);
				testModels.Remove(tm);
				await testModelRepository.Save(testModels);

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
	}

	
}