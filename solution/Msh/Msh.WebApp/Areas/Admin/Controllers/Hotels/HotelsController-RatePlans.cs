using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Models.RoomTypes;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

public partial class HotelsController
{
	[Route("RatePlanList")]
	public async Task<IActionResult> RatePlanList([FromQuery] string hotelCode = "")
	{
		var vm = new RatePlanListVm
		{
			HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode,
			HotelName = string.Empty
		};

		try
		{
			await Task.Delay(0);

			vm.Hotels = await hotelRepository.GetData();

			var hotel = string.IsNullOrEmpty(hotelCode)
				? vm.Hotels.FirstOrDefault()
				: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

			vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
			vm.HotelName = hotel != null ? hotel.Name : string.Empty;

			var ratePlans = (await ratePlanRepository.GetData(vm.HotelCode)) ?? [];

			vm.RatePlans = ratePlans;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			//if (!string.IsNullOrEmpty(vm.HotelCode))
			//{
			//	await ratePlanRepository.SaveMissingConfigAsync($"{ConstHotel.Cache.RoomTypes}-{vm.HotelCode}", new List<RoomType>());
			//}

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
	[Route("RatePlanAdd")]
	public async Task<IActionResult> RatePlanAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View(new RoomRatePlan
		{
			StayFrom = DateOnly.FromDateTime(DateTime.Now),
			StayTo = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
			BookFrom = DateOnly.FromDateTime(DateTime.Now),
			BookTo = DateOnly.FromDateTime(DateTime.Now).AddDays(1),
		});
	}

	[HttpPost]
	[Route("RatePlanAdd")]
	public async Task<IActionResult> RatePlanAdd([FromForm]RoomRatePlan ratePlan, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var ratePlans = await ratePlanRepository.GetData(hotelCode);

			if (ratePlans.All(tm => tm.RatePlanCode != ratePlan.RatePlanCode))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				ratePlan.Notes = string.IsNullOrEmpty(ratePlan.Notes) ? string.Empty : ratePlan.Notes;
				
				ratePlans.Add(ratePlan);
				await ratePlanRepository.Save(ratePlans, hotelCode);
				return RedirectToAction(nameof(RatePlanAdd), new { IsSuccess = true, HotelCode = hotelCode, Code = ratePlan.RatePlanCode });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(ratePlan);
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(ratePlan);
		}
	}


	[HttpGet]
	[Route("RatePlanEdit")]
	public async Task<IActionResult> RatePlanEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var ratePlans = await ratePlanRepository.GetData(hotelCode);
		var roomType = ratePlans.FirstOrDefault(m => m.RatePlanCode == code);
		if (roomType != null)
		{
			return View(roomType);
		}

		return RedirectToAction(nameof(RoomTypeList));
	}

	[HttpPost]
	[Route("RatePlanEdit")]
	public async Task<IActionResult> RatePlanEdit([FromForm] RoomRatePlan roomType, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var ratePlans = await ratePlanRepository.GetData(hotelCode);
			var index = ratePlans.FindIndex(m => m.RatePlanCode == roomType.RatePlanCode);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				// roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				ratePlans[index] = roomType;

				await ratePlanRepository.Save(ratePlans, hotelCode);

				return RedirectToAction(nameof(RatePlanEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = roomType.RatePlanCode });
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
	[Route("RatePlanMove")]
	public async Task<IActionResult> RatePlanMove([FromBody] ApiInput input)
	{
		try
		{

			var hotelCode = input.HotelCode;
			var hotels = await hotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcItems = await ratePlanRepository.GetData(hotelCode);
			var currentIndex = srcItems.FindIndex(item => item.RatePlanCode.EqualsAnyCase(input.Code));
			var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
			var currentItem = srcItems[currentIndex];
			var swapItem = srcItems[swapIndex];
			srcItems[swapIndex] = currentItem;
			srcItems[currentIndex] = swapItem;
			await ratePlanRepository.Save(srcItems, hotelCode);

			var table = new RatePlanListVm
			{
				Hotels = hotels,
				HotelCode = hotelCode,
				RatePlans = srcItems
			};

			return PartialView("Hotels/_RatePlansTable", table);

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}