using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services.Cache;

public partial interface IHotelCacheService
{
    Task<List<Hotel>> GetConfigHotels();

    void ReloadHotels();
}