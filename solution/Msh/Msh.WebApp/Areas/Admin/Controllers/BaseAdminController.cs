using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Msh.Common.Models.ViewModels;
using Msh.HotelCache.Services;

namespace Msh.WebApp.Areas.Admin.Controllers;

public class BaseAdminController(IHotelRepository hotelRepository) : Controller
{
	protected readonly IHotelRepository HotelRepository = hotelRepository;

	protected List<SelectListItem> GetLanguages()
	{
		var gEurope = new SelectListGroup { Name = "Europe" };
		var gAsia = new SelectListGroup { Name = "Asia" };
		return [
			new SelectListItem { Text = "English", Selected = true, Group = gEurope},
			new SelectListItem { Text = "French", Disabled = true, Group = gEurope },
			new SelectListItem { Text = "German", Group = gEurope },
			new SelectListItem { Text = "Chinese", Group = gAsia },
			new SelectListItem { Text = "Japanese", Group = gAsia }
		];
	}

	protected async Task<List<SelectListItem>> GetHotels()
	{
		var hotels = await HotelRepository.GetData();

		var list = new List<SelectListItem>();

		foreach (var h in hotels)
		{
			list.Add(new SelectListItem { Value = h.HotelCode, Text = h.Name });
		}

		return list;
	}

	protected IActionResult GetFail(string message) => Ok(new ObjectVm { Success = false, UserErrorMessage = message });
}