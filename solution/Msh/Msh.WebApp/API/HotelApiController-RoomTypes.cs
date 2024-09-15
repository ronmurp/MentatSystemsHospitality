using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Models.RoomTypes;
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

	[HttpPost]
	[Route("RoomTypeCopyBulk")]
	public async Task<IActionResult> RoomTypeCopyBulk(ApiInput input)
	{
		try
		{
			if (SameCodes(input))
			{
				return GetFail("At least one code must change");
			}

			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.NewHotelCode)))
			{
				return GetFail($"Invalid destination hotel code {input.NewHotelCode}");
			}

			var missingList = new List<string>();
			var newList = new List<RoomType>();

			var srcExtras = await hotelsRepoService.GetRoomTypesAsync(input.HotelCode);
			var dstExtras = await hotelsRepoService.GetRoomTypesAsync(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var extra = srcExtras.FirstOrDefault(h => h.Code == code);
				if (extra != null)
				{
					if (dstExtras.Any(e => e.Code == extra.Code))
					{
						// Already exists
						missingList.Add(extra.Code);
						continue;
					}
					newList.Add(extra);
				}
			}

			dstExtras.AddRange(newList);

			await hotelsRepoService.SaveRoomTypesAsync(dstExtras, input.NewHotelCode);

			if (missingList.Count > 0)
			{
				var list = string.Join(",", missingList);
				return GetFail($"The following codes already exist in the destination hotel: {list}");

			}

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

	[HttpPost]
	[Route("RoomTypeDeleteBulk")]
	public async Task<IActionResult> RoomTypeDeleteBulk(ApiInput input)
	{
		try
		{
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await hotelsRepoService.GetRoomTypesAsync(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await hotelsRepoService.SaveRoomTypesAsync(items, input.HotelCode);



			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}


	[HttpPost]
	[Route("RoomTypesSort")]
	public async Task<IActionResult> RoomTypesSort(ApiInput input)
	{
		try
		{
			var hotelCode = input.HotelCode;
			var hotels = await hotelsRepoService.GetHotelsAsync();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var srcExtras = await hotelsRepoService.GetRoomTypesAsync(hotelCode);

			await hotelsRepoService.SaveRoomTypesAsync(srcExtras.OrderBy(e => e.Code)
				.ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}