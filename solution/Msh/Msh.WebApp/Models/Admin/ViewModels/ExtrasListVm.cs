using Msh.HotelCache.Models.Discounts;
using Msh.HotelCache.Models.Extras;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class ExtrasListVm : HotelListVm
{
	public List<Extra> Extras { get; set; } = [];
}

public class DiscountsListVm : HotelListVm
{
	public List<DiscountCode> Discounts { get; set; } = [];
}