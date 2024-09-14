using Msh.HotelCache.Models.Hotels;

namespace Msh.WebApp.Models.Admin.ViewModels;

/// <summary>
/// Basic view model that contains hotel information
/// </summary>
public class HotelListVm
{
	public string HotelCode { get; set; } = string.Empty;

	public List<Hotel> Hotels { get; set; } = [];

	public string HotelName { get; set; } = string.Empty;

	public string ErrorMessage { get; set; } = string.Empty;
}