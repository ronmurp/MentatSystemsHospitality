using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services.Cache;

/// <summary>
/// Return (or reload) Hotel RoomTypes
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public partial class HotelCacheService
{
    public async Task<List<RoomType>> GetRoomTypes(string hotelCode) =>
        await base.GetData<List<RoomType>>(GetCacheName(ConstHotel.Cache.RoomTypes, hotelCode));

    public void ReloadRoomTypes(string hotelCode) => 
        base.Reload(GetCacheName(ConstHotel.Cache.RoomTypes, hotelCode));

    public async Task<List<HotelRoomTypesFilters>> GetRoomTypeFilters(string hotelCode) =>
        await base.GetData<List<HotelRoomTypesFilters>>(GetCacheName(ConstHotel.Cache.RoomTypeFilters, hotelCode));

    public void ReloadRoomTypeFilters(string hotelCode) =>
        base.Reload(GetCacheName(ConstHotel.Cache.RoomTypeFilters, hotelCode));
}