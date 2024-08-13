using Msh.HotelCache.Models;

namespace Msh.HotelCache.Services.Cache;

public partial interface IHotelCacheService
{
    Task<List<Hotel>> GetConfigHotels();

    void ReloadHotels();
}