using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

public partial class HotelsController
{
	[Route("RatePlansTextList")]
	public async Task<IActionResult> RatePlansTextList([FromQuery] string hotelCode = "")
	{
		var vm = new RatePlanTextListVm
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

			var ratePlansText = (await ratePlanTextRepository.GetData(vm.HotelCode)) ?? [];

			vm.RatePlansText = ratePlansText;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			//if (!string.IsNullOrEmpty(vm.HotelCode))
			//{
			//	await ratePlanRepository.SaveMissingConfigAsync($"{ConstHotel.Cache.RoomTypes}-{vm.HotelCode}", new List<RoomType>());
			//}

			vm.ErrorMessage = $"No rate plan texts for hotel {vm.HotelCode}";

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
	[Route("RatePlanTextAdd")]
	public async Task<IActionResult> RatePlanTextAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;


		return View(new RatePlanText
		{
			
		});
	}


	[HttpPost]
	[Route("RatePlanTextAdd")]
	public async Task<IActionResult> RatePlanTextAdd([FromForm]RatePlanText ratePlan, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		//var rp = ratePlan.Adapt<RoomRatePlan>();

		if (ModelState.IsValid)
		{
			

			var ratePlanText = await ratePlanTextRepository.GetData(hotelCode);

			if (ratePlanText.All(tm => tm.Id != ratePlan.Id))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				ratePlan.Notes = string.IsNullOrEmpty(ratePlan.Notes) ? string.Empty : ratePlan.Notes;
				
				ratePlanText.Add(ratePlan);
				await ratePlanTextRepository.Save(ratePlanText, hotelCode);
				return RedirectToAction(nameof(RatePlanTextAdd), new { IsSuccess = true, HotelCode = hotelCode, Id = ratePlan.Id });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Id already exists");

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
	[Route("RatePlanTextEdit")]
	public async Task<IActionResult> RatePlanTextEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var ratePlans = await ratePlanTextRepository.GetData(hotelCode);
		var ratePlan = ratePlans.FirstOrDefault(m => m.Id == code);
		if (ratePlan != null)
		{
			var rp = ratePlan.Adapt<RatePlanText>();
			return View(rp);
		}

		return RedirectToAction(nameof(RatePlansTextList));
	}

	[HttpPost]
	[Route("RatePlanTextEdit")]
	public async Task<IActionResult> RatePlanTextEdit([FromForm] RatePlanText ratePlan, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		//var rp = ratePlan.Adapt<RoomRatePlan>();

		if (ModelState.IsValid)
		{
			var ratePlans = await ratePlanTextRepository.GetData(hotelCode);
			var index = ratePlans.FindIndex(m => m.Id == ratePlan.Id);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				// roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				ratePlans[index] = ratePlan;

				await ratePlanTextRepository.Save(ratePlans, hotelCode);

				return RedirectToAction(nameof(RatePlanTextEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = ratePlan.Id });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code does not exist");

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


	//[HttpPost]
	//[Route("RatePlanMove")]
	//public async Task<IActionResult> RatePlanMove([FromBody] ApiInput input)
	//{
	//	try
	//	{

	//		var hotelCode = input.HotelCode;
	//		var hotels = await hotelRepository.GetData();
	//		if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
	//		{
	//			return GetFail($"Invalid hotel code {hotelCode}");
	//		}

	//		var srcItems = await ratePlanRepository.GetData(hotelCode);
	//		var currentIndex = srcItems.FindIndex(item => item.RatePlanCode.EqualsAnyCase(input.Code));
	//		var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
	//		var currentItem = srcItems[currentIndex];
	//		var swapItem = srcItems[swapIndex];
	//		srcItems[swapIndex] = currentItem;
	//		srcItems[currentIndex] = swapItem;
	//		await ratePlanRepository.Save(srcItems, hotelCode);

	//		var table = new RatePlanListVm
	//		{
	//			Hotels = hotels,
	//			HotelCode = hotelCode,
	//			RatePlans = srcItems
	//		};

	//		return PartialView("Hotels/_RatePlansTable", table);

	//	}
	//	catch (Exception ex)
	//	{
	//		return GetFail(ex.Message);
	//	}
	//}


}