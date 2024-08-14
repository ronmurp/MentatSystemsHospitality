using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services.Cache;

/// <summary>
/// Return (or reload) Hotel
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public partial class HotelCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
    : DataCacheService(memoryCache, configRepository), IHotelCacheService
{
    public async Task<List<Hotel>> GetConfigHotels() =>
        await base.GetData<List<Hotel>>(ConstHotel.Cache.Hotel);

    public void ReloadHotels() => base.Reload(ConstHotel.Cache.Hotel);

}