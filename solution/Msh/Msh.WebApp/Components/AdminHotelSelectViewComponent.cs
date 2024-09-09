using Microsoft.AspNetCore.Mvc;
using Msh.HotelCache.Services;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Components;

/// <summary>
/// Presents an *Admin* hotel select based on database config data
/// </summary>
/// <param name="hotelsRepoService"></param>
public class AdminHotelSelectViewComponent(IHotelsRepoService hotelsRepoService) : ViewComponent
{
	/// <summary>
	/// Invoke the view components with the selected hotel code
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	public async Task<IViewComponentResult> InvokeAsync(string hotelCode)
	{
		var hotels = await hotelsRepoService.GetHotelsAsync();

		var vm = new HotelListVm
		{
			HotelCode = hotelCode,
			Hotels = hotels
		};

		return View(vm);
	}
}