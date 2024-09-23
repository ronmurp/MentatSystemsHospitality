using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{
	Task<List<RoomType>> GetRoomTypesAsync(string hotelCode);

	Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode);
}