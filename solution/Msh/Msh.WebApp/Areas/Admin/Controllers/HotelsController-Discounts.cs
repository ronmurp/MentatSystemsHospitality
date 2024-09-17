using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.RoomTypes;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers;

public partial class HotelsController
{
	[Route("DiscountsList")]
	public async Task<IActionResult> DiscountsList([FromQuery] string hotelCode = "")
	{
		var vm = new DiscountsListVm
		{
			HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode,
			HotelName = string.Empty
		};

		try
		{

			vm.Hotels = await hotelsRepoService.GetHotelsAsync();
			var hotel = string.IsNullOrEmpty(hotelCode)
				? vm.Hotels.FirstOrDefault()
				: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

			vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
			vm.HotelName = hotel != null ? hotel.Name : string.Empty;

			var discounts = (await hotelsRepoService.GetDiscountCodesAsync(vm.HotelCode)) ?? [];

			vm.Discounts = discounts;

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
	[Route("DiscountAdd")]
	public async Task<IActionResult> DiscountAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View(new DiscountCode());
	}

	[HttpPost]
	[Route("DiscountAdd")]
	public async Task<IActionResult> DiscountAdd([FromForm]DiscountCode extra, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		if (ModelState.IsValid)
		{
			var discounts = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);

			if (discounts.All(tm => tm.Code != extra.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				//roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				discounts.Add(extra);
				await hotelsRepoService.SaveDiscountCodesAsync(discounts, hotelCode);
				return RedirectToAction(nameof(DiscountAdd), new { IsSuccess = true, HotelCode = hotelCode, Code = extra.Code });
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
	[Route("DiscountEdit")]
	public async Task<IActionResult> DiscountEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var discounts = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);
		var discount = discounts.FirstOrDefault(m => m.Code == code);
		if (discount != null)
		{
			return View(discount);
		}

		return RedirectToAction(nameof(DiscountsList));
	}

	[HttpPost]
	[Route("DiscountEdit")]
	public async Task<IActionResult> DiscountEdit([FromForm] DiscountCode discount, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var discounts = await hotelsRepoService.GetDiscountCodesAsync(hotelCode);
			var index = discounts.FindIndex(m => m.Code == discount.Code);
			if (index >= 0)
			{
				// Keep other properties not in the main edit
				discount.Notes = string.IsNullOrEmpty(discount.Notes) ? string.Empty : discount.Notes;
				discount.OfferDates = discounts[index].OfferDates; //
				discount.BookDates = discounts[index].BookDates; //
				discount.DayRates = discounts[index].DayRates;
				discount.DisabledHotelPlans = discounts[index].DisabledHotelPlans;
				discount.EnabledHotelPlans = discounts[index].EnabledHotelPlans;
				discount.DiscountErrors = discounts[index].DiscountErrors;
				discount.OneTime = discounts[index].OneTime;
				// Now update
				discounts[index] = discount;
				// And save
				await hotelsRepoService.SaveDiscountCodesAsync(discounts, hotelCode);
				return RedirectToAction(nameof(DiscountEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = discount.Code });
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


	[Route("DiscountEditDatesOffer")]
	public async Task<IActionResult> DiscountEditDatesOffer(string hotelCode, string code, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			
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

	[Route("DiscountEditDatesBook")]
	public async Task<IActionResult> DiscountEditDatesBook(string hotelCode, string code, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);


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

}