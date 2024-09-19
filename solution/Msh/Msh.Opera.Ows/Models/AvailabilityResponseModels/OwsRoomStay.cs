namespace Msh.Opera.Ows.Models.AvailabilityResponseModels;

/// <summary>
/// Room stay, as returned by OWS
/// </summary>
public class OwsRoomStay
{
	public List<OwsRatePlan> OwsRatePlans { get; set; } = [];
	public List<OwsRoomType> OwsRoomTypes { get; set; } = [];
	public List<OwsRoomRate> OwsRoomRates { get; set; } = [];
}