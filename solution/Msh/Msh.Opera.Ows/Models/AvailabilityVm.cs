using Msh.Opera.Ows.Services.CustomTest;

namespace Msh.Opera.Ows.Models;

/// <summary>
/// View model for Admin page AvailabilityLinks.aspx
/// </summary>
public class AvailabilityVm : BaseAdminVm
{
	public AvailabilityVm()
	{
		Arrive = DateTime.Now.Date;
		Depart = Arrive.AddDays(1);
	}
	public DateTime Arrive { get; set; }
	public DateTime Depart { get; set; }

	public string ArriveText => $"{Arrive:yyyy-MM-dd}";
	public string DepartText => $"{Depart:yyyy-MM-dd}";
	public List<CustomAvailabilityService.RoomRate> List { get; set; } = new List<CustomAvailabilityService.RoomRate>();
	public List<CustomAvailabilityService.RoomType> ListRoomTypes { get; set; } = new List<CustomAvailabilityService.RoomType>();
	public List<CustomAvailabilityService.RatePlan> ListRatePlans { get; set; } = new List<CustomAvailabilityService.RatePlan>();
}