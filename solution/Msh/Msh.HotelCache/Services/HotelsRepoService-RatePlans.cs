using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{
	public async Task<List<RoomRatePlan>> GetRatePlansAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<RoomRatePlan>>(ConstHotel.Cache.RatePlans, hotelCode) ?? [];


	public async Task SaveRatePlansAsync(List<RoomRatePlan> ratePlans, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.RatePlans, hotelCode, ratePlans);
	}

}