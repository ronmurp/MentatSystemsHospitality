namespace Msh.WebApp.API.Admin.Hotels;

public class RatePlanEditStayVm
{
	public string HotelCode { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;

	public DateOnly StayFrom { get; set; }

	public DateOnly StayTo { get; set; }

	public string RatePlanCode { get; set; } = string.Empty;
}