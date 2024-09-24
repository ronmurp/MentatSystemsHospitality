using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.Exceptions;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.Areas.Admin.Controllers.Hotels;

[Authorize]
[Area("Admin")]
[Route("admin/hotels")]
public partial class HotelsController(ILogger<HotelsController> logger,
	IHotelRepository hotelRepository,
	IDiscountRepository discountRepository,
	IExtraRepository extraRepository,
	IRatePlanRepository ratePlanRepository,
	IRoomTypeRepository roomTypeRepository,
	ISpecialsRepository specialsRepository,
	ITestModelRepository testModelRepository,
	IUserService userService) : Controller
{
	
	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);

		var users = userService.GetUsers();

		var userId = userService.GetUserId() ?? string.Empty;

		return View("Index", userId);
	}


	[Route("HotelList")]
	public async Task<IActionResult> HotelList()
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelRepository.GetData();

			return View(hotels);
		}
		catch (NullConfigException ex)
		{
			//await hotelRepository.SaveMissingConfigAsync(ConstHotel.Cache.Hotel, new List<Hotel>());


			return View(new List<Hotel>());
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View(new List<Hotel>());
		}
	}


	[Route("HotelAdd")]
	public async Task<IActionResult> HotelAdd(bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.IsSuccess = isSuccess;
			ViewBag.Code = string.Empty;

			return View(new Hotel());
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[HttpPost]
	[Route("HotelAdd")]
	public async Task<IActionResult> HotelAdd([FromForm] HotelBase hotel)
	{
		try
		{
			await Task.Delay(0);

			if (!ModelState.IsValid)
			{
				// Invalid data, so return to form, with model values
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

				return View(hotel);

			}

			var hotelList = await hotelRepository.GetData();
			var h = hotelList.FirstOrDefault(x => x.HotelCode == hotel.HotelCode);

			if (h != null)
			{
				// This hotel code already exists
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Code already exists");

				return View(hotel);
			}

			// Success ... show success and clear form ready to add another

			var newHotel = hotel.Adapt<Hotel>();

			hotelList.Add(newHotel);

			await hotelRepository.Save(hotelList);

			return RedirectToAction(nameof(HotelAdd), new { IsSuccess = true, HotelCode = hotel.HotelCode });


		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}


	[Route("HotelEdit/{hotelCode}")]
	public async Task<IActionResult> HotelEdit(string hotelCode, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.IsSuccess = isSuccess;
			ViewBag.Code = string.Empty;
			ViewBag.HotelCode = hotelCode;

			var hotels = await hotelRepository.GetData();
			var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
			return View(hotel);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[HttpPost]
	[Route("HotelEdit/{hotelCode}")]
	public async Task<IActionResult> HotelEdit([FromForm]HotelBase hotel, string hotelCode = "")
	{
		try
		{
			await Task.Delay(0);

			if (!ModelState.IsValid)
			{
				// Invalid data, so return to form, with model values
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", ConstHotel.Vem.GeneralSummary);

				return View(hotel);

			}

			var hotelList = await hotelRepository.GetData();
			var index = hotelList.FindIndex(x => x.HotelCode == hotel.HotelCode);

			if (index < 0)
			{
				// This hotel code already exists
				ViewBag.IsSuccess = false;
				ViewBag.Code = string.Empty;

				ModelState.AddModelError("", "That Hotel Code does not exists");

				return View(hotel);
			}

			// Success ... show success and clear form ready to add another

			var updatedHotel = hotel.Adapt<Hotel>();
			// Must keep date list (this form does not edit the list)
			updatedHotel.StayDates = hotelList[index].StayDates;
			hotelList[index] = updatedHotel;

			await hotelRepository.Save(hotelList);

			return RedirectToAction(nameof(HotelEdit), new { IsSuccess = true, HotelCode = hotel.HotelCode });



		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}


	[Route("HotelStayEditDates/{hotelCode}")]
	public async Task<IActionResult> HotelStayEditDates(string hotelCode, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

			ViewBag.HotelCode = hotelCode;

			return View();
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[Route("HotelBookEditDates/{hotelCode}")]
	public async Task<IActionResult> HotelBookEditDates(string hotelCode, bool isSuccess = false)
	{
		try
		{
			await Task.Delay(0);

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
	[Route("HotelMove")]
	public async Task<IActionResult> HotelMove([FromBody] ApiInput input)
	{
		try
		{

			var srcItems = await hotelRepository.GetData();

			var currentIndex = srcItems.FindIndex(item => item.HotelCode.EqualsAnyCase(input.Code));
			var swapIndex = input.Direction == 0 ? currentIndex - 1 : currentIndex + 1;
			var currentItem = srcItems[currentIndex];
			var swapItem = srcItems[swapIndex];
			srcItems[swapIndex] = currentItem;
			srcItems[currentIndex] = swapItem;
			await hotelRepository.Save(srcItems);

			return PartialView("Hotels/_HotelsTable", srcItems);

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	private IActionResult GetFail(string message)
	{
		return Ok(new ObjectVm { Success = false, UserErrorMessage = message });
	}
}
