using Msh.HotelCache.Models.RatePlans;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class RatePlanListVm : HotelListVm
{
	public List<RoomRatePlan> RatePlans { get; set; } = [];
}