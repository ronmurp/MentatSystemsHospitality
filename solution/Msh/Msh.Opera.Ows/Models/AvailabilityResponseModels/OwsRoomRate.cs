namespace Msh.Opera.Ows.Models.AvailabilityResponseModels;

/// <summary>
/// RoomRate, as returned by OWS, but with NumberOfUnits co-pied from OwsRoomTypes
/// </summary>
public class OwsRoomRate
{
	public string RoomTypeCode { get; set; } = string.Empty;
	public string RatePlanCode { get; set; } = string.Empty;
	public decimal Total { get; set; }
	public int NumberOfUnits { get; set; }
	public string CurrencyCode { get; set; } = string.Empty;
}