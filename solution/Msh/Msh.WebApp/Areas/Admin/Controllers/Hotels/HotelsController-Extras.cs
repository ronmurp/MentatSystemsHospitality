using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.RoomTypes;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

public partial class HotelsController
{
	[Route("ExtrasList")]
	public async Task<IActionResult> ExtrasList([FromQuery] string hotelCode = "")
	{
		var vm = new ExtrasListVm
		{
			HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode,
			HotelName = string.Empty
		};

		try
		{
			await Task.Delay(0);

			vm.Hotels = await hotelsRepoService.GetHotelsAsync();
			var hotel = string.IsNullOrEmpty(hotelCode)
				? vm.Hotels.FirstOrDefault()
				: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

			vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
			vm.HotelName = hotel != null ? hotel.Name : string.Empty;

			var roomTypes = (await hotelsRepoService.GetExtrasAsync(vm.HotelCode)) ?? [];

			vm.Extras = roomTypes;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			if (!string.IsNullOrEmpty(vm.HotelCode))
			{
				await configRepository.SaveMissingConfigAsync(ConstHotel.Cache.Extras, vm.HotelCode, new List<RoomType>());
			}

			vm.ErrorMessage = $"No extras for hotel {vm.HotelCode}";

			return View(vm);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			vm.ErrorMessage = $"Error for hotel {vm.HotelCode}. {ex.Message}";
			return View(vm);
		}
	}


	[HttpGet]
	[Route("ExtraAdd")]
	public async Task<IActionResult> ExtraAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View(new Extra());
	}

	[HttpPost]
	[Route("ExtraAdd")]
	public async Task<IActionResult> ExtraAdd([FromForm]Extra extra, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		if (ModelState.IsValid)
		{
			var extras = await hotelsRepoService.GetExtrasAsync(hotelCode);

			if (extras.All(tm => tm.Code != extra.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				//roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				extras.Add(extra);
				await hotelsRepoService.SaveExtrasAsync(extras, hotelCode);
				return RedirectToAction(nameof(ExtraAdd), new { IsSuccess = true, HotelCode = hotelCode, Code = extra.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(extra);
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(extra);
		}
	}


	[HttpGet]
	[Route("ExtraEdit")]
	public async Task<IActionResult> ExtraEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var extras = await hotelsRepoService.GetExtrasAsync(hotelCode);
		var extra = extras.FirstOrDefault(m => m.Code == code);
		if (extra != null)
		{
			return View(extra);
		}

		return RedirectToAction(nameof(ExtrasList));
	}

	[HttpPost]
	[Route("ExtraEdit")]
	public async Task<IActionResult> ExtraEdit([FromForm] Extra extra, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var extras = await hotelsRepoService.GetExtrasAsync(hotelCode);
			var index = extras.FindIndex(m => m.Code == extra.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				// extra.Notes = string.IsNullOrEmpty(extra.Notes) ? string.Empty : extra.Notes;

				extras[index] = extra;
				await hotelsRepoService.SaveExtrasAsync(extras, hotelCode);
				return RedirectToAction(nameof(ExtraEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = extra.Code });
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


	[Route("ExtraEditDates")]
	public async Task<IActionResult> ExtraEditDates(string hotelCode, string code, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			//var hotels = await hotelsRepoService.GetHotelsAsync();
			//var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
			//if (hotel == null)
			//{
			//	return RedirectToAction("HotelList");
			//}
			ViewBag.Code = code;
			ViewBag.HotelCode = hotelCode;

			return View();
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[HttpPost]
	[Route("ExtraMove")]
	public async Task<IActionResult> ExtraMove([FromBody] ApiInput input)
	{
		try
		{

			var hotelCode = input.HotelCode;
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcItems = await hotelsRepoService.GetExtrasAsync(hotelCode);

			var currentIndex = srcItems.FindIndex(item => item.Code.EqualsAnyCase(input.Code));
			var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
			var currentItem = srcItems[currentIndex];
			var swapItem = srcItems[swapIndex];
			srcItems[swapIndex] = currentItem;
			srcItems[currentIndex] = swapItem;
			await hotelsRepoService.SaveExtrasAsync(srcItems, hotelCode);

			var table = new ExtrasListVm
			{
				Hotels = hotels,
				HotelCode = hotelCode,
				Extras = srcItems
			};

			return PartialView("Hotels/_ExtrasTable", table);

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}