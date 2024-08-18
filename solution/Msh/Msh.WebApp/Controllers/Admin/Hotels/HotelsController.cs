﻿using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.Exceptions;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Services;

namespace Msh.WebApp.Controllers.Admin.Hotels
{
	[Route("admin/hotels")]
	public class HotelsController(ILogger<HomeController> logger, 
		IHotelsRepoService hotelsRepoService,
		IConfigRepository configRepository) : Controller
	{
		[Route("")]
		public async Task<IActionResult> Index()
		{
			await Task.Delay(0);

			return View("~/Views/Admin/Hotels/Index.cshtml");
		}

		[Route("HotelList")]
		public async Task<IActionResult> HotelList()
		{
			try
			{
				await Task.Delay(0);

				var hotels = hotelsRepoService.GetHotels();

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

				var hotels = hotelsRepoService.GetHotels();
				var hotel = new Hotel();
				hotels.Add(hotel);
				return RedirectToAction("HotelEdit", hotel);
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

		[Route("HotelEditByCode")]
		public async Task<IActionResult> HotelEditByCode([FromQuery]string hotelCode, [FromQuery]string action)
		{
			try
			{
				await Task.Delay(0);

				var hotels = hotelsRepoService.GetHotels();
				if (action == "delete")
				{
					var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);
					hotels.Remove(hotel);
					hotelsRepoService.SaveHotels(hotels);
					return RedirectToAction("HotelList");
				}
				else
				{
					var hotel = hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

					return RedirectToAction("HotelEdit", hotel);
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

				var hotelList = hotelsRepoService.GetHotels();
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
					hotelsRepoService.SaveHotels(hotelList);
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
}
