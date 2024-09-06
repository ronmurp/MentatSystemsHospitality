using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.Common.Exceptions;
using Msh.Common.Models.ViewModels;
using Msh.Common.Services;
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

				var hotels = hotelsRepoService.GetHotels();
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
				

				var hotels = hotelsRepoService.GetHotels();
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
					hotelsRepoService.SaveHotels(hotels);
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

		[Route("RoomTypeList")]
		public async Task<IActionResult> RoomTypeList([FromQuery] string hotelCode)
		{
			var vm = new RoomTypeListVm
			{
				HotelCode = string.IsNullOrEmpty(hotelCode) ? string.Empty : hotelCode
			};

			try
			{
				await Task.Delay(0);

				vm.Hotels = hotelsRepoService.GetHotels();
				var hotel = string.IsNullOrEmpty(hotelCode)
					? vm.Hotels.FirstOrDefault()
					: vm.Hotels.FirstOrDefault(h => h.HotelCode == hotelCode);

				vm.HotelCode = hotel != null ? hotel.HotelCode : string.Empty;

				var roomTypes = hotelsRepoService.GetRoomTypes(vm.HotelCode);

				vm.RoomTypes = roomTypes;

				return View("~/Views/Admin/Hotels/RoomTypeList.cshtml", vm);
			}
			catch (NullConfigException ex)
			{
				if (!string.IsNullOrEmpty(vm.HotelCode))
				{
					configRepository.SaveMissingConfig(ConstHotel.Cache.RoomTypes, vm.HotelCode, new List<RoomType>());
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

		
		
	}

	[ApiController]
	[Route("api/hotelapi")]
	public class HotelApiController : Controller
	{
		[HttpGet]
		[Route("hotelSave")]
		public async Task<IActionResult> HotelSave()
		{
			try
			{
				await Task.Delay(0);

				return Ok(new ObjectVm
				{
					Data = new Hotel()
				});
			}
			catch (Exception ex)
			{
				return Ok(new ObjectVm
				{
					Success = false,
					UserErrorMessage = ex.Message
				});
			}
		}

		[HttpPost]
		[Route("hotelSave")]
		public async Task<IActionResult> HotelSave(Hotel hotel)
		{
			try
			{
				await Task.Delay(0);

				return Ok(new ObjectVm
				{
					Data = hotel
				});
			}
			catch (Exception ex)
			{
				return Ok(new ObjectVm
				{
					Success = false,
					UserErrorMessage = ex.Message
				});
			}
		}

		
		[HttpGet]
		[Route("hotelConfig")]
		public async Task<IActionResult> HotelConfig()
		{
			return await GetConfig(typeof(Hotel));
		}

		[HttpGet]
		[Route("roomTypeConfig")]
		public async Task<IActionResult> RoomTypeConfig()
		{
			return await GetConfig(typeof(RoomType));
		}

		private async Task<IActionResult> GetConfig(Type classType)
		{
			try
			{
				await Task.Delay(0);

				var propService = new PropertyValueService();
				var list = propService.GetProperties(classType);
				return Ok(new ObjectVm
				{
					Data = list
				});
			}
			catch (Exception ex)
			{
				return Ok(new ObjectVm
				{

					UserErrorMessage = ex.Message
				});
			}
		}

	}
}
