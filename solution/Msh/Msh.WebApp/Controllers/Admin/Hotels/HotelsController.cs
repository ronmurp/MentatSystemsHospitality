using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.Exceptions;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;
using Microsoft.AspNetCore.Authorization;
using Msh.WebApp.Services;

namespace Msh.WebApp.Controllers.Admin.Hotels;

[Authorize]
[Route("admin/hotels")]
public partial class HotelsController(ILogger<HomeController> logger, 
	IHotelsRepoService hotelsRepoService,
	IConfigRepository configRepository,
	IUserService userService) : Controller
{
	[Route("")]
	public async Task<IActionResult> Index()
	{
		await Task.Delay(0);

		var users = userService.GetUsers();

		var userId = userService.GetUserId() ?? string.Empty;

		return View("~/Views/Admin/Hotels/Index.cshtml", userId);
	}

	[Route("HotelList")]
	public async Task<IActionResult> HotelList()
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelsRepoService.GetHotelsAsync();

			return View("~/Views/Admin/Hotels/HotelList.cshtml", hotels);
		}
		catch (NullConfigException ex)
		{
			configRepository.SaveMissingConfig(ConstHotel.Cache.Hotel, new List<Hotel>());


			return View("~/Views/Admin/Hotels/HotelList.cshtml", new List<Hotel>());
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
			return View("~/Views/Admin/Hotels/HotelList.cshtml", new List<Hotel>());
		}
	}

	[Route("HotelAdd")]
	public async Task<IActionResult> HotelAdd()
	{
		try
		{
			await Task.Delay(0);

			return View("~/Views/Admin/Hotels/HotelAdd.cshtml");
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[Route("HotelEdit")]
	public async Task<IActionResult> HotelEdit(Hotel hotel)
	{
		try
		{
			await Task.Delay(0);

			return View("~/Views/Admin/Hotels/HotelEdit.cshtml", hotel);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[Route("HotelEdit/{hotelCode}")]
	public async Task<IActionResult> HotelEdit(string hotelCode)
	{
		try
		{
			await Task.Delay(0);

			var hotels = await hotelsRepoService.GetHotelsAsync();
			var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
			return View("~/Views/Admin/Hotels/HotelEdit.cshtml", hotel);
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[Route("HotelEditByCode")]
	public async Task<IActionResult> HotelEditByCode([FromQuery]string hotelCode, [FromQuery]string action)
	{
		try
		{
			await Task.Delay(0);
				

			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (action == "add")
			{
				var hotel = new Hotel();
				hotels.Add(hotel);
				return RedirectToAction("HotelEdit", hotel);
			}
			if (action == "delete")
			{
				var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
				hotels.Remove(hotel);
				await hotelsRepoService.SaveHotelsAsync(hotels);
				return RedirectToAction("HotelList");
			}
			else
			{
				var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

				return Redirect($"HotelEdit/{hotelCode}");
			}
				

		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	[HttpPost]
	[Route("HotelSave")]
	public async Task<IActionResult> HotelSave(Hotel hotel)
	{
		try
		{
			await Task.Delay(0);

			if (!ModelState.IsValid)
			{
				return RedirectToAction("HotelEdit", hotel);
			}

			var hotelList = await hotelsRepoService.GetHotelsAsync();
			var found = false;
			for (var i = 0; i < hotelList.Count; i++)
			{
				if (hotelList[i].HotelCode == hotel.HotelCode)
				{
					hotelList[i] = hotel;
					found = true;
				}
			}

			if (!found)
			{
				hotelList.Add(hotel);
				found = true;
			}

			if (found)
			{
				await hotelsRepoService.SaveHotelsAsync(hotelList);
			}


			return RedirectToAction("HotelList");
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}


	[HttpPost]
	[Route("HotelSave2")]
	public async Task<IActionResult> HotelSave2(Hotel hotel)
	{
		try
		{
			await Task.Delay(0);

			if (!ModelState.IsValid)
			{
				return Ok(new ObjectVm
				{
					Success = false

				});

			}

			var hotelList = await hotelsRepoService.GetHotelsAsync();
			var found = false;
			for (var i = 0; i < hotelList.Count; i++)
			{
				if (hotelList[i].HotelCode == hotel.HotelCode)
				{
					hotelList[i] = hotel;
					found = true;
				}
			}

			if (!found)
			{
				hotelList.Add(hotel);
				found = true;
			}

			if (found)
			{
				await hotelsRepoService.SaveHotelsAsync(hotelList);
			}


			return RedirectToAction("HotelList");
		}
		catch (Exception ex)
		{
			logger.LogError($"{ex.Message}");
		}

		return RedirectToAction("HotelList");
	}

	

}
