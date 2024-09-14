using Msh.HotelCache.Models.Extras;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class ExtrasListVm : HotelListVm
{
	public List<Extra> Extras { get; set; } = [];
}