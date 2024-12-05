using Msh.WebApp.Areas.Admin.Data;

namespace Msh.WebApp.Models.Admin.ViewModels;

public class SpecialOptionListVm
{
	public string HotelCode { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;

	public List<SpecialOptionsVm> List { get; set; } = [];
}

