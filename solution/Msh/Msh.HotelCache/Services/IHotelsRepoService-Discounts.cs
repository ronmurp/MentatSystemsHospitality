using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{
	Task<List<DiscountCode>> GetDiscountCodesAsync(string hotelCode);

	Task SaveDiscountCodesAsync(List<DiscountCode> discountCodes, string hotelCode);
}