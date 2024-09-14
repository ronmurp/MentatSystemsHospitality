using Msh.HotelCache.Models.RoomTypes;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class RoomTypeListVm : HotelListVm
{
	public List<RoomType> RoomTypes { get; set; } = [];
}