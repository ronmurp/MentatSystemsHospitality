using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Models.Specials;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Areas.Admin.Controllers;

public partial class HotelsController
{
	[Route("SpecialsList")]
	public async Task<IActionResult> SpecialsList([FromQuery] string hotelCode = "")
	{
		var vm = new SpecialsListVm
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

			var items = (await hotelsRepoService.GetSpecialsAsync(vm.HotelCode)) ?? [];

			vm.Specials = items;

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
	[Route("SpecialAdd")]
	public async Task<IActionResult> SpecialAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View(new Special());
	}

	[HttpPost]
	[Route("SpecialAdd")]
	public async Task<IActionResult> SpecialAdd([FromForm]Special special, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		if (ModelState.IsValid)
		{
			var items = await hotelsRepoService.GetSpecialsAsync(hotelCode);

			if (items.All(tm => tm.Code != special.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				special.Notes = string.IsNullOrEmpty(special.Notes) ? string.Empty : special.Notes;

				items.Add(special);
				await hotelsRepoService.SaveSpecialsAsync(items, hotelCode);
				return RedirectToAction(nameof(SpecialEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = special.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(special);
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(special);
		}
	}


	[HttpGet]
	[Route("SpecialEdit")]
	public async Task<IActionResult> SpecialEdit(string code, string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		var specials = await hotelsRepoService.GetSpecialsAsync(hotelCode);
		var special = specials.FirstOrDefault(m => m.Code == code);
		if (special != null)
		{
			return View(special);
		}

		return RedirectToAction(nameof(ExtrasList));
	}

	[HttpPost]
	[Route("SpecialEdit")]
	public async Task<IActionResult> SpecialEdit([FromForm] Special special, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var items = await hotelsRepoService.GetSpecialsAsync(hotelCode);
			var index = items.FindIndex(m => m.Code == special.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				special.Notes = string.IsNullOrEmpty(special.Notes) ? string.Empty : special.Notes;
				special.Options = items[index].Options;
				special.ItemDates = items[index].ItemDates;
				special.RoomTypeCodes = items[index].RoomTypeCodes;
				special.RatePlanCodes = items[index].RatePlanCodes;

				items[index] = special;
				await hotelsRepoService.SaveSpecialsAsync(items, hotelCode);
				return RedirectToAction(nameof(SpecialEdit), new { IsSuccess = true, HotelCode = hotelCode, Code = special.Code });
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


	[Route("SpecialEditDates")]
	public async Task<IActionResult> SpecialEditDates(string hotelCode, string code, bool isSuccess = false)
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

		return RedirectToAction("SpecialsList");
	}

	[Route("SpecialEditOptions")]
	public async Task<IActionResult> SpecialEditOptions(string hotelCode, string code, bool isSuccess = false)
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

		return RedirectToAction("SpecialsList");
	}

	[Route("SpecialEditRoomTypes")]
	public async Task<IActionResult> SpecialEditRoomTypes(string hotelCode, string code, bool isSuccess = false)
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

			var roomTypes = await hotelsRepoService.GetRoomTypesAsync(hotelCode);
			var specials = await hotelsRepoService.GetSpecialsAsync(hotelCode);
			var special = specials.FirstOrDefault(s => s.Code.EqualsAnyCase(code));
			if (special != null)
			{
				var selected = special.RoomTypeCodes;

				foreach (var rt in roomTypes.OrderBy(x => x.GroupCode).ThenBy(x => x.Code).ToList())
				{
					var rtr = new CodeCheckListRow
					{
						Code = rt.Code,
						GroupCode = rt?.GroupCode ?? string.Empty,
						Name = rt?.Name ?? string.Empty,
						Selected = selected.Any(r => r.EqualsAnyCase(rt.Code))
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

		return RedirectToAction("SpecialsList");
	}

	[Route("SpecialEditRatePlans")]
	public async Task<IActionResult> SpecialEditRatePlans(string hotelCode, string code, bool isSuccess = false)
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

			var roomTypes = await hotelsRepoService.GetRatePlansAsync(hotelCode);
			var specials = await hotelsRepoService.GetSpecialsAsync(hotelCode);
			var special = specials.FirstOrDefault(s => s.Code.EqualsAnyCase(code));
			if (special != null)
			{
				var selected = special.RoomTypeCodes;

				foreach (var rt in roomTypes.OrderBy(x => x.Group).ThenBy(x => x.RatePlanCode).ToList())
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

		return RedirectToAction("SpecialsList");
	}

	[HttpPost]
	[Route("SpecialMove")]
	public async Task<IActionResult> SpecialMove([FromBody]ApiInput input)
	{
		try
		{
			
			var hotelCode = input.HotelCode;
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcItems = await hotelsRepoService.GetSpecialsAsync(hotelCode);
			var currentIndex = srcItems.FindIndex(item => item.Code.EqualsAnyCase(input.Code));
			var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
			var currentItem = srcItems[currentIndex];
			var swapItem = srcItems[swapIndex];
			srcItems[swapIndex] = currentItem;
			srcItems[currentIndex] = swapItem;
			await hotelsRepoService.SaveSpecialsAsync(srcItems, hotelCode);

			var table = new SpecialsListVm
			{
				Hotels = hotels,
				HotelCode = hotelCode,
				Specials = srcItems
			};

			return PartialView("Hotels/_SpecialsTable", table);

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	
}