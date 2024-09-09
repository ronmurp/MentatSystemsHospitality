using Microsoft.AspNetCore.Mvc;
using Msh.HotelCache.Services;
using Msh.WebApp.Models.Admin.ViewModels;

namespace Msh.WebApp.Components;

public class AdminHotelSelectViewComponent(IHotelsRepoService hotelsRepoService) : ViewComponent
{
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