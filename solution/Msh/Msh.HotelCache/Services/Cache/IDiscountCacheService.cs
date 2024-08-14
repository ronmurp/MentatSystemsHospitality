using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Services.Cache;

public partial interface IDiscountCacheService
{
    Task<List<DiscountCode>> GetDiscountCodes(string groupCode);

    void ReloadDiscountCodes(string groupCode);

    Task<List<DiscountGroup>> GetDiscountGroups();


    void ReloadDiscountGroups();

}