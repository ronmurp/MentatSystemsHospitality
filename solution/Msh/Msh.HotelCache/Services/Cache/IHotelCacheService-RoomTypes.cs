using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services.Cache;

public partial interface IHotelCacheService
{
    Task<List<RoomType>> GetRoomTypes(string hotelCode);

    void ReloadRoomTypes(string hotelCode);

    Task<List<HotelRoomTypesFilters>> GetRoomTypeFilters(string hotelCode);

    void ReloadRoomTypeFilters(string hotelCode);
}