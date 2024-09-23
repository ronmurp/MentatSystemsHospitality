using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{

	public async Task<List<RoomType>> GetRoomTypesAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<RoomType>>(ConstHotel.Cache.RoomTypes, hotelCode) ?? [];

	public async Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.RoomTypes, hotelCode, roomTypes);
	}

}