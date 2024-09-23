using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{

	Task<List<RoomRatePlan>> GetRatePlansAsync(string hotelCode);

	Task SaveRatePlansAsync(List<RoomRatePlan> ratePlans, string hotelCode);
}