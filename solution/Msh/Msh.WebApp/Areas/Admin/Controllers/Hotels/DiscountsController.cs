using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Services;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

[Authorize]
[Area("Admin")]
[Route("admin/Discounts")]
public class DiscountsController(ILogger<HotelsController> logger,
	IHotelRepository hotelRepository,
	IDiscountRepository discountRepository,
	IRatePlanRepository ratePlanRepository,
	IUserService userService) : BaseAdminController(hotelRepository)
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

			vm.Hotels = await HotelRepository.GetData();
			var hotel = string.IsNullOrEmpty(hotelCode)
				? vm.Hotels.FirstOrDefault()
				: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

			vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
			vm.HotelName = hotel != null ? hotel.Name : string.Empty;

			var discounts = (await discountRepository.GetData(vm.HotelCode)) ?? [];

			vm.Discounts = discounts;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			if (!string.IsNullOrEmpty(vm.HotelCode))
			{
				await discountRepository.Save( new List<DiscountCode>(), vm.HotelCode);
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
	public async Task<IActionResult> DiscountAdd([FromForm]DiscountCode discountCode, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		if (ModelState.IsValid)
		{
			var discounts = await discountRepository.GetData(hotelCode);

			if (discounts.All(tm => tm.Code != discountCode.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				//roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				discounts.Add(discountCode);

				await discountRepository.Save(discounts, hotelCode);

				return RedirectToAction(nameof(DiscountAdd), new { IsSuccess = true, HotelCode = hotelCode, Code = discountCode.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(discountCode);
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(discountCode);
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

		var discounts = await discountRepository.GetData(hotelCode);
		var discount = discounts.FirstOrDefault(m => m.Code == code);
		if (discount != null)
		{
			return View(discount);
		}

		return RedirectToAction(nameof(DiscountsList));
	}

	[HttpPost]
	[Route("DiscountEdit")]
	public async Task<IActionResult> DiscountEdit([FromForm] DiscountCode discountCode, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var discounts = await discountRepository.GetData(hotelCode);
			var index = discounts.FindIndex(m => m.Code == discountCode.Code);
			if (index >= 0)
			{
				// Prevent nullifying other properties not in the main edit
				discountCode.Notes = string.IsNullOrEmpty(discountCode.Notes) ? string.Empty : discountCode.Notes;
				discountCode.OfferDates = discounts[index].OfferDates; //
				discountCode.BookDates = discounts[index].BookDates; //
				discountCode.DayRates = discounts[index].DayRates;
				discountCode.DisabledHotelPlans = discounts[index].DisabledHotelPlans;
				discountCode.EnabledHotelPlans = discounts[index].EnabledHotelPlans;
				discountCode.DiscountErrors = discounts[index].DiscountErrors;
				discountCode.OneTime = discounts[index].OneTime;
				// Now that passed parameter has been updated with properties not edited, update the original
				discounts[index] = discountCode;
				// And save
				await discountRepository.Save(discounts, hotelCode);
				return RedirectToAction(nameof(DiscountEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = discountCode.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code does not exist");

				return View(new DiscountCode());
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(new DiscountCode());
		}
	}

	//[HttpGet]
	//[Route("DiscountEditErrors")]
	//public async Task<IActionResult> DiscountEditErrors(string code, string hotelCode, bool isSuccess = false)
	//{
	//	await Task.Delay(0);

	//	ViewBag.IsSuccess = isSuccess;
	//	ViewBag.Code = code;
	//	ViewBag.Languages = GetLanguages();
	//	ViewBag.Hotels = await GetHotels();
	//	ViewBag.HotelCode = hotelCode;

	//	var discounts = await discountRepository.GetData(hotelCode);
	//	var discount = discounts.FirstOrDefault(m => m.Code == code);
	//	if (discount != null)
	//	{
	//		return View(discount.DiscountErrors);
	//	}

	//	return RedirectToAction(nameof(DiscountsList));
	//}

	//[HttpPost]
	//[Route("DiscountEditErrors")]
	//public async Task<IActionResult> DiscountEditErrors([FromForm] DiscountErrors discountErrors, string hotelCode, string code)
	//{
	//	ViewBag.Languages = GetLanguages();
	//	ViewBag.Hotels = await GetHotels();

	//	if (ModelState.IsValid)
	//	{
	//		var discounts = await discountRepository.GetData(hotelCode);
	//		var index = discounts.FindIndex(m => m.Code == code);
	//		if (index >= 0)
	//		{
				
	//			// Now that passed parameter has been updated with properties not edited, update the original
	//			discounts[index].DiscountErrors = discountErrors;
	//			// And save
	//			await discountRepository.Save(discounts, hotelCode);

	//			return RedirectToAction(nameof(DiscountEditErrors), new { IsSuccess = true, HotelCode = hotelCode, Code = code });
	//		}
	//		else
	//		{
	//			ViewBag.IsSuccess = false;
	//			ViewBag.Code = string.Empty;

	//			ModelState.AddModelError("", "That Code does not exist");

	//			return View(new DiscountErrors());
	//		}
	//	}
	//	else
	//	{
	//		ViewBag.IsSuccess = false;
	//		ViewBag.Code = string.Empty;

	//		ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

	//		return View(new DiscountErrors());
	//	}
	//}


	[HttpGet]
	[Route("DiscountEditOneTime")]
	public async Task<IActionResult> DiscountEditOneTime(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = code;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var discounts = await discountRepository.GetData(hotelCode);

		var discount = discounts.FirstOrDefault(m => m.Code == code);
		if (discount != null)
		{
			return View(discount.OneTime);
		}

		return RedirectToAction(nameof(DiscountsList));
	}

	[HttpPost]
	[Route("DiscountEditOneTime")]
	public async Task<IActionResult> DiscountEditOneTime([FromForm] DiscountOneTime discountOneTime, string hotelCode, string code)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var discounts = await discountRepository.GetData(hotelCode);
			var index = discounts.FindIndex(m => m.Code == code);
			if (index >= 0)
			{

				// Now that passed parameter has been updated with properties not edited, update the original
				discounts[index].OneTime = discountOneTime;
				// And save
				await discountRepository.Save(discounts, hotelCode);

				return RedirectToAction(nameof(DiscountEditOneTime), new { IsSuccess = true, HotelCode = hotelCode, Code = code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code does not exist");

				return View(new DiscountOneTime());
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(new DiscountOneTime());
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

		return RedirectToAction("HotelsList");
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

		return RedirectToAction("HotelsList");
	}

	[Route("DiscountEditRatePlansEnable")]
	public async Task<IActionResult> DiscountEditRatePlansEnable(string hotelCode, string code, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.Code = code;
			ViewBag.HotelCode = hotelCode;

			var vm = new CodeCheckListVm
			{
				HotelCode = hotelCode,
				Code = code
			};

			var ratePlans = await ratePlanRepository.GetData(hotelCode);
			var discounts = await discountRepository.GetData(hotelCode);
			var discount = discounts.FirstOrDefault(s => s.Code.EqualsAnyCase(code));
			if (discount != null)
			{
				var selected = discount.EnabledHotelPlans;

				foreach (var rt in ratePlans.OrderBy(x => x.Group).ThenBy(x => x.RatePlanCode).ToList())
				{
					var rtr = new CodeCheckListRow
					{
						Code = rt.RatePlanCode,
						GroupCode = rt?.Group ?? string.Empty,
						Name = rt?.Description ?? string.Empty,
						Selected = selected.Any(r => r.EqualsAnyCase(rt.RatePlanCode))
					};
					vm.List.Add(rtr);
				}
			}

			return View(vm);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("DiscountsList");
	}

	[Route("DiscountEditRatePlansDisable")]
	public async Task<IActionResult> DiscountEditRatePlansDisable(string hotelCode, string code, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.Code = code;
			ViewBag.HotelCode = hotelCode;

			var vm = new CodeCheckListVm
			{
				HotelCode = hotelCode,
				Code = code
			};

			var ratePlans = await ratePlanRepository.GetData(hotelCode);
			var discounts = await discountRepository.GetData(hotelCode);
			var discount = discounts.FirstOrDefault(s => s.Code.EqualsAnyCase(code));
			if (discount != null)
			{
				var selected = discount.DisabledHotelPlans;

				foreach (var rt in ratePlans.OrderBy(x => x.Group).ThenBy(x => x.RatePlanCode).ToList())
				{
					var rtr = new CodeCheckListRow
					{
						Code = rt.RatePlanCode,
						GroupCode = rt?.Group ?? string.Empty,
						Name = rt?.Description ?? string.Empty,
						Selected = selected.Any(r => r.EqualsAnyCase(rt.RatePlanCode))
					};
					vm.List.Add(rtr);
				}
			}

			return View(vm);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("DiscountsList");
	}


	[Route("DiscountErrors")]
	public async Task<IActionResult> DiscountErrors(string hotelCode, string code, bool isSuccess = false)
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

		return RedirectToAction("DiscountsList");
	}
}