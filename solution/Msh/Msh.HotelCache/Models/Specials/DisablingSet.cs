namespace Msh.HotelCache.Models.Specials;

public class DisablingSet
{
	public List<string> RoomTypeCodes { get; set; } = [];
	public List<string> RatePlanCodes { get; set; } = [];
	public bool AdultsOnly { get; set; }
	public string DisabledText { get; set; } = string.Empty;
}