using Microsoft.AspNetCore.Mvc;
using Msh.Common.Models.ViewModels;
using Msh.WebApp.Areas.Admin.Models;

namespace Msh.WebApp.API;

public partial class HotelApiController
{

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