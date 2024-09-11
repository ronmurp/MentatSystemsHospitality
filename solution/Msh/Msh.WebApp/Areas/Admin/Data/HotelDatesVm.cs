using Msh.Common.Models.Dates;

namespace Msh.WebApp.Areas.Admin.Data;

public class HotelDatesVm
{
	public string HotelCode { get; set; } = string.Empty;
	public List<HotelDateItem> Dates { get; set; } = [];
}