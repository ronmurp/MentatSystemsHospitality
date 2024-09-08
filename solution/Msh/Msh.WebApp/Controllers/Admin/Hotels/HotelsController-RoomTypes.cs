using Microsoft.AspNetCore.Mvc;
using Msh.Common.Exceptions;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Models;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Controllers.Admin.Hotels;

public partial class HotelsController
{
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



}