using Msh.Opera.Ows.Models.AvailabilityResponseModels;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsRoomTypeRes : OwsRoomType{
	public string? Description { get; set; }
	public string? ShortDescription { get; set; }
}