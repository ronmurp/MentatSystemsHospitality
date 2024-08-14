using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Services.Cache;

public partial class DiscountCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
    : DataCacheService(memoryCache, configRepository), IDiscountCacheService
{
    public async Task<List<DiscountCode>> GetDiscountCodes(string groupCode) =>
        await base.GetData<List<DiscountCode>>(GetCacheName(ConstHotel.Cache.DiscountCodes, groupCode));

    public void ReloadDiscountCodes(string groupCode) =>
        base.Reload(GetCacheName(ConstHotel.Cache.DiscountCodes, groupCode));

    public async Task<List<DiscountGroup>> GetDiscountGroups() =>
        await base.GetData<List<DiscountGroup>>(ConstHotel.Cache.DiscountGroups);

    public void ReloadDiscountGroups() => base.Reload(ConstHotel.Cache.DiscountGroups);
}