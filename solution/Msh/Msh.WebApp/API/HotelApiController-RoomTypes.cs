using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API;
public partial class HotelApiController
{
	[HttpPost]
	[Route("RoomTypeCopy")]
	public async Task<IActionResult> RoomTypeCopy(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}
			var roomTypes = await hotelsRepoService.GetRoomTypesAsync(input.HotelCode);
			var roomType = roomTypes.FirstOrDefault(h => h.Code == input.Code);
			if (roomType != null)
			{
				var newRoomType = roomType.Adapt(roomType);
				newRoomType.Code = input.NewCode;

				var result = await CheckHotel(input.NewHotelCode);
				if (!result.success)
				{
					return GetFail("The hotel does not exist.");
				}

				var newRoomTypes = await hotelsRepoService.GetRoomTypesAsync(input.NewHotelCode);
				if (newRoomTypes.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newRoomTypes.Add(newRoomType);
				await hotelsRepoService.SaveRoomTypesAsync(newRoomTypes, input.NewHotelCode);
			}

			return Ok(new ObjectVm());
			
		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}


	[HttpPost]
	[Route("RoomTypeDelete")]
	public async Task<IActionResult> RoomTypeDelete(ApiInput input)
	{
		try
		{
			var roomTypes = await hotelsRepoService.GetRoomTypesAsync(input.HotelCode);
			var roomType = roomTypes.FirstOrDefault(h => h.Code == input.Code);
			if (roomType != null)
			{
				roomTypes.Remove(roomType);
				await hotelsRepoService.SaveRoomTypesAsync(roomTypes, input.HotelCode);
			}

			return Ok(new ObjectVm
			{

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
}