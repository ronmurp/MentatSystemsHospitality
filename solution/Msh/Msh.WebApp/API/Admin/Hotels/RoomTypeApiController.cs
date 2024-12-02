using Mapster;
using Microsoft.AspNetCore.Mvc;
using Msh.Common.ExtensionMethods;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Models.RoomTypes;
using Msh.HotelCache.Services;
using Msh.WebApp.Areas.Admin.Models;
using Msh.WebApp.Services;

namespace Msh.WebApp.API.Admin.Hotels;

[ApiController]
[Route("api/roomtypeapi")]
public partial class RoomTypeApiController : PrivateApiController
{
	private const string ModelName = "Room Types";

	private readonly IUserService _userService;
	private readonly IRoomTypeRepository _roomTypeRepository;

	public RoomTypeApiController(IHotelRepository hotelRepository,
		IUserService userService,
		IRoomTypeRepository roomTypeRepository) : base(hotelRepository)
	{
		_userService = userService;
		_roomTypeRepository = roomTypeRepository;
	}
	[HttpGet]
	[Route("RoomTypeConfig")]
	public async Task<IActionResult> RoomTypeConfig()
	{
		return await GetConfig(typeof(RoomType));
	}

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
			var roomTypes = await _roomTypeRepository.GetData(input.HotelCode);
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

				var newRoomTypes = await _roomTypeRepository.GetData(input.NewHotelCode);
				if (newRoomTypes.Any(c => c.Code.EqualsAnyCase(input.NewCode)))
				{
					return GetFail("The code already exists.");
				}

				newRoomTypes.Add(newRoomType);
				await _roomTypeRepository.Save(newRoomTypes, input.NewHotelCode);
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
			var roomTypes = await _roomTypeRepository.GetData(input.HotelCode);
			var roomType = roomTypes.FirstOrDefault(h => h.Code == input.Code);
			if (roomType != null)
			{
				roomTypes.Remove(roomType);
				await _roomTypeRepository.Save(roomTypes, input.HotelCode);
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

			var hotels = await HotelRepository.GetData();

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

			var srcItems = await _roomTypeRepository.GetData(input.HotelCode);
			var dstItems = await _roomTypeRepository.GetData(input.NewHotelCode);

			foreach (var code in input.CodeList)
			{
				var extra = srcItems.FirstOrDefault(h => h.Code == code);
				if (extra != null)
				{
					if (dstItems.Any(e => e.Code == extra.Code))
					{
						// Already exists
						missingList.Add(extra.Code);
						continue;
					}
					newList.Add(extra);
				}
			}

			dstItems.AddRange(newList);

			await _roomTypeRepository.Save(dstItems, input.NewHotelCode);

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
			var hotels = await HotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(input.HotelCode)))
			{
				return GetFail($"Invalid source hotel code {input.HotelCode}");
			}

			var items = await _roomTypeRepository.GetData(input.HotelCode);

			for (var i = items.Count - 1; i >= 0; i--)
			{
				var item = items[i];
				if (input.CodeList.Any(c => c.EqualsAnyCase(item.Code)))
				{
					items.RemoveAt(i);
				}
			}

			await _roomTypeRepository.Save(items, input.HotelCode);

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
			var hotels = await HotelRepository.GetData();
			if (!hotels.Any(h => h.HotelCode.EqualsAnyCase(hotelCode)))
			{
				return GetFail($"Invalid hotel code {hotelCode}");
			}

			var items = await _roomTypeRepository.GetData(hotelCode);

			await _roomTypeRepository.Save(items.OrderBy(e => e.Code).ToList(), hotelCode);

			return Ok(new ObjectVm());

		}
		catch (Exception ex)
		{
			return GetFail(ex.Message);
		}
	}

}