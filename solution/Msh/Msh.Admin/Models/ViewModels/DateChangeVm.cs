namespace Msh.WebApp.API.Admin.Hotels;

/// <summary>
/// A view model for working with Rate Plan date ranges
/// </summary>
public class DateChangeVm
{
	public DateOnly DateFrom { get; set; }

	public DateOnly DateTo { get; set; }

	public bool IsFrom { get; set; }
}