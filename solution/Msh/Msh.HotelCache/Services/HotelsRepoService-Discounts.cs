using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{
	public async Task<List<DiscountCode>> GetDiscountCodesAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<DiscountCode>>(ConstHotel.Cache.Discounts, hotelCode) ?? [];

	public async Task SaveDiscountCodesAsync(List<DiscountCode> discountCodes, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Discounts, hotelCode, discountCodes);
	}
}