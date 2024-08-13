using Msh.HotelCache.Models;

namespace Msh.HotelCache.Services.Cache;

/// <summary>
/// Return (or reload) Hotel Rate Plans
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public partial class HotelCacheService
{
    public async Task<List<RoomRatePlan>> GetRatePlans(string hotelCode) =>
        await base.GetData<List<RoomRatePlan>>(GetCacheName(ConstHotel.Cache.RatePlans, hotelCode));

    public void ReloadHotels(string hotelCode) => base.Reload(GetCacheName(ConstHotel.Cache.RatePlans, hotelCode));

}