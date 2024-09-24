using Microsoft.AspNetCore.Mvc;
using Msh.Common.Data;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models;
using Msh.HotelCache.Services;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Components;

/// <summary>
/// Presents an *Admin* hotel select based on database config data
/// </summary>
/// <param name="hotelRepository"></param>
public class AdminHotelSelectViewComponent(IConfigRepository configRepository, IHotelRepository hotelRepository) : ViewComponent
{
	/// <summary>
	/// Invoke the view components with the selected hotel code
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	public async Task<IViewComponentResult> InvokeAsync(string hotelCode)
	{
		var hotels = await configRepository.GetConfigContentAsync<List<Hotel>>(ConstHotel.Cache.Hotel);

		var vm = new HotelListVm
		{
			HotelCode = hotelCode,
			Hotels = hotels
		};

		return View(vm);
	}
}