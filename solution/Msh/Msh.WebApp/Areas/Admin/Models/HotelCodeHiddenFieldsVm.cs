namespace Msh.WebApp.Areas.Admin.Models;

/// <summary>
/// View model for _HotelCodeHiddenFields.cshtml
/// </summary>
public class HotelCodeHiddenFieldsVm
{
	public bool UseHotel { get; set; }
	public string? HotelCode { get; set; } = string.Empty;

	public string? Code { get; set; } = string.Empty;
	public string? EditType { get; set; }
}