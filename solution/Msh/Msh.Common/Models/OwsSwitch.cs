namespace Msh.Common.Models;

public class OwsSwitch
{
	public string? HotelCode { get; set; }

	public List<SwitchOperation> Services { get; set; } = [];
}