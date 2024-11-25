using Msh.HotelCache.Models.RatePlans;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class RatePlanListVm : HotelListVm
{
	public List<RoomRatePlan> RatePlans { get; set; } = [];
}

public class RatePlanTextListVm : HotelListVm
{
	public List<RatePlanText> RatePlansText { get; set; } = [];
}

public class RatePlanSortListVm : HotelListVm
{
	public List<RatePlanSortVm> RatePlanSorts { get; set; } = [];
}