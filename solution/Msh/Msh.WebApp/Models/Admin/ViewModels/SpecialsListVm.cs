using Msh.HotelCache.Models.Specials;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class SpecialsListVm : HotelListVm
{
	public List<Special> Specials { get; set; } = [];
}