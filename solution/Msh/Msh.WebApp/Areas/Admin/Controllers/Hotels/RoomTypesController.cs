using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.Admin.Models.Const;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Models.Admin.ViewModels;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

[Authorize]
[Area("Admin")]
[Route(AdminRoutes.RoomTypes)]
public class RoomTypesController(ILogger<HotelsController> logger,
	IHotelRepository hotelRepository,
	IRoomTypeRepository roomTypeRepository,
	IRatePlanRepository ratePlanRepository,
	IUserService userService) : BaseAdminController(hotelRepository)
{
	[Route("RoomTypesList")]
	public async Task<IActionResult> RoomTypesList([FromQuery] string hotelCode = "")
	{
		var vm = new RoomTypeListVm
		{
			HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode,
			HotelName = string.Empty
		};

		try
		{
			await Task.Delay(0);

			vm.Hotels = await HotelRepository.GetData();

			var hotel = string.IsNullOrEmpty(hotelCode)
				? vm.Hotels.FirstOrDefault()
				: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

			vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;
			vm.HotelName = hotel != null ? hotel.Name : string.Empty;

			var roomTypes = (await roomTypeRepository.GetData(vm.HotelCode)) ?? [];

			vm.RoomTypes = roomTypes;

			return View(vm);
		}
		catch (NullConfigException ex)
		{
			//if (!string.IsNullOrEmpty(vm.HotelCode))
			//{
			//	await roomTypeRepository.SaveMissingConfigAsync($"{ConstHotel.Cache.RoomTypes}-{vm.HotelCode}", new List<RoomType>());
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
	[Route("RoomTypeAdd")]
	public async Task<IActionResult> RoomTypeAdd(string hotelCode, bool isSuccess = false)
	{
		await Task.Delay(0);

		ViewBag.IsSuccess = isSuccess;
		ViewBag.Code = string.Empty;
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();
		ViewBag.HotelCode = hotelCode;

		return View(new RoomType());
	}

	[HttpPost]
	[Route("RoomTypeAdd")]
	public async Task<IActionResult> RoomTypeAdd([FromForm]RoomType roomType, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var roomTypes = await roomTypeRepository.GetData(hotelCode);

			if (roomTypes.All(tm => tm.Code != roomType.Code))
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				//roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				roomTypes.Add(roomType);
				await roomTypeRepository.Save(roomTypes, hotelCode);
				return RedirectToAction(nameof(RoomTypeAdd), new { IsSuccess = true, HotelCode = hotelCode, Code = roomType.Code });
			}
			else
			{
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(roomType);
			}
		}
		else
		{
			ViewBag.IsSuccess = false;
			ViewBag.Code = string.Empty;

			ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

			return View(roomType);
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

		var roomTypes = await roomTypeRepository.GetData(hotelCode);
		var roomType = roomTypes.FirstOrDefault(m => m.Code == code);
		if (roomType != null)
		{
			return View(roomType);
		}

		return RedirectToAction(nameof(RoomTypesList));
	}

	[HttpPost]
	[Route("RoomTypeEdit")]
	public async Task<IActionResult> RoomTypeEdit([FromForm] RoomType roomType, string hotelCode)
	{
		ViewBag.Languages = GetLanguages();
		ViewBag.Hotels = await GetHotels();

		if (ModelState.IsValid)
		{
			var roomTypes = await roomTypeRepository.GetData(hotelCode);
			var index = roomTypes.FindIndex(m => m.Code == roomType.Code);
			if (index >= 0)
			{
				//testModel.Hotels = testModel.Hotels.Where(m => !string.IsNullOrEmpty(m)).ToList();
				// roomType.Notes = string.IsNullOrEmpty(roomType.Notes) ? string.Empty : roomType.Notes;

				roomTypes[index] = roomType;
				await roomTypeRepository.Save(roomTypes, hotelCode);
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


	[HttpPost]
	[Route("RoomTypeMove")]
	public async Task<IActionResult> RoomTypeMove([FromBody] ApiInput input)
	{
		try
		{

			var hotelCode = input.HotelCode;
			var hotels = await HotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcItems = await roomTypeRepository.GetData(hotelCode);

			var currentIndex = srcItems.FindIndex(item => item.Code.EqualsAnyCase(input.Code));
			var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
			var currentItem = srcItems[currentIndex];
			var swapItem = srcItems[swapIndex];
			srcItems[swapIndex] = currentItem;
			srcItems[currentIndex] = swapItem;
			await roomTypeRepository.Save(srcItems, hotelCode);

			var table = new RoomTypeListVm()
			{
				Hotels = hotels,
				HotelCode = hotelCode,
				RoomTypes = srcItems
			};

			return PartialView("Hotels/_RoomTypesTable", table);

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}
}