using Msh.HotelCache.Models;

namespace Msh.HotelCache.Services.Cache;

public partial interface IHotelCacheService
{
    Task<List<RoomRatePlan>> GetRatePlans(string hotelCode);

    void ReloadHotels(string hotelCode);
}