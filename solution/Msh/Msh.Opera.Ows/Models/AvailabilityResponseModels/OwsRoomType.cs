namespace Msh.Opera.Ows.Models.AvailabilityResponseModels;

/// <summary>
/// Room type, as returned by OWS
/// </summary>
public class OwsRoomType
{
	public string? RoomTypeCode { get; set; }
	public int NumberOfUnits { get; set; }
}