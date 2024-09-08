using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.Exceptions;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;
using Msh.WebApp.Models.Admin.ViewModels;
using System.Collections.Generic;

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

				var hotels = hotelsRepoService.GetHotelsAsync();

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

		[Route("RoomTypeList")]
		public async Task<IActionResult> RoomTypeList([FromQuery] string hotelCode)
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

				var roomTypes = await hotelsRepoService.GetRoomTypesAsync(vm.HotelCode);

				vm.RoomTypes = roomTypes;

				return View("~/Views/Admin/Hotels/RoomTypeList.cshtml", vm);
			}
			catch (NullConfigException ex)
			{
				if (!string.IsNullOrEmpty(vm.HotelCode))
				{
					await configRepository.SaveMissingConfigAsync(ConstHotel.Cache.RoomTypes, vm.HotelCode, new List<RoomType>());
				}

				vm.ErrorMessage = $"No room types for hotel {vm.HotelCode}";
				
				return View("~/Views/Admin/Hotels/RoomTypeList.cshtml", vm);
			}
			catch (Exception ex)
			{
				logger.LogError($"{ex.Message}");
				vm.ErrorMessage = $"Error for hotel {vm.HotelCode}. {ex.Message}";
				return View("~/Views/Admin/Hotels/RoomTypeList.cshtml", vm);
			}
		}


		[Route("TestModelList")]
		public async Task<IActionResult> TestModelList()
		{
			try
			{
				await Task.Delay(0);

				var hotels = await hotelsRepoService.GetTestModelsAsync();

				return View("~/Views/Admin/Hotels/TestModelList.cshtml", hotels);
			}
			catch (NullConfigException ex)
			{
				configRepository.SaveMissingConfig(ConstHotel.Cache.Hotel, new List<TestModel>());


				return View("~/Views/Admin/Hotels/TestModelList.cshtml", new List<TestModel>());
			}
			catch (Exception ex)
			{
				logger.LogError($"{ex.Message}");
				return View("~/Views/Admin/Hotels/TestModelList.cshtml", new List<Hotel>());
			}
		}
		[HttpGet]
		[Route("TestModelAdd")]
		public async Task<IActionResult> TestModelAdd(bool isSuccess = false)
		{
			await Task.Delay(0);

			ViewBag.IsSuccess = isSuccess;

			//if (!string.IsNullOrEmpty(code))
			//{
			//	var testModels = hotelsRepoService.GetTestModelsAsync();
			//	var tm = testModels.FirstOrDefault(x => x.Code == code);
			//	if (tm != null)
			//	{
			//		return View("~/Views/Admin/Hotels/TestModelAdd.cshtml", tm);
			//	}
			//}

			return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
		}

		[HttpPost]
		[Route("TestModelAdd")]
		public async Task<IActionResult> TestModelAdd(TestModel testModel)
		{
			var testModels = await hotelsRepoService.GetTestModelsAsync();

			if (testModels.All(tm => tm.Code != testModel.Code))
			{
				testModels.Add(testModel);
				await hotelsRepoService.SaveTestModelsAsync(testModels);
				return RedirectToAction(nameof(TestModelAdd), new { IsSuccess = true });
			}

			return View("~/Views/Admin/Hotels/TestModelAdd.cshtml");
		}

	}
}
