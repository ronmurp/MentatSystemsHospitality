using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers;

public partial class HotelsController
{
	[Route("RoomTypeList")]
	public async Task<IActionResult> RoomTypeList([FromQuery] string hotelCode = "")
	{
		var vm = new RoomTypeListVm
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

			var roomTypes = (await hotelsRepoService.GetRoomTypesAsync(vm.HotelCode)) ?? [];

			vm.RoomTypes = roomTypes;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			if (!string.IsNullOrEmpty(vm.HotelCode))
			{
				await configRepository.SaveMissingConfigAsync(ConstHotel.Cache.RoomTypes, vm.HotelCode, new List<RoomType>());
			}

			vm.ErrorMessage = $"No room types for hotel {vm.HotelCode}";

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
	[Route("RoomTypeAdd")]
	public async Task<IActionResult> RoomTypeAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View();
	}


	[HttpPost]
	[Route("RoomTypeAdd")]
	public async Task<IActionResult> RoomTypeAdd([FromForm]RoomType roomType, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var roomTypes = await hotelsRepoService.GetRoomTypesAsync(hotelCode);

			if (roomTypes.All(tm => tm.Code != roomType.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				//roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				roomTypes.Add(roomType);
				await hotelsRepoService.SaveRoomTypesAsync(roomTypes, hotelCode);
				return RedirectToAction(nameof(RoomTypeAdd), new { IsSuccess = true, Code = roomType.Code });
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
	[Route("RoomTypeEdit")]
	public async Task<IActionResult> RoomTypeEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var roomTypes = await hotelsRepoService.GetRoomTypesAsync(hotelCode);
		var roomType = roomTypes.FirstOrDefault(m => m.Code == code);
		if (roomType != null)
		{
			return View(roomType);
		}

		return RedirectToAction(nameof(RoomTypeList));
	}


	[HttpPost]
	[Route("RoomTypeEdit")]
	public async Task<IActionResult> RoomTypeEdit([FromForm] RoomType roomType, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var roomTypes = await hotelsRepoService.GetRoomTypesAsync(hotelCode);
			var index = roomTypes.FindIndex(m => m.Code == roomType.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				// roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				roomTypes[index] = roomType;
				await hotelsRepoService.SaveRoomTypesAsync(roomTypes, hotelCode);
				return RedirectToAction(nameof(RoomTypeEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = roomType.Code });
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

}