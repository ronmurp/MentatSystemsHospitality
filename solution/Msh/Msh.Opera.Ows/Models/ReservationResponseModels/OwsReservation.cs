using Msh.Common.Models.OwsCommon;
using Msh.Opera.Ows.Models.ReservationRequestModels;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS in response to a Create/Modify reservation
/// </summary>
public class OwsReservation
{
	public List<OwsUniqueId> OwsUniqueIds { get; set; }

	public string ReservationId => 
		OwsUniqueIds
			.SingleOrDefault(uid => uid.Type == OwsUniqueIdType.INTERNAL && string.IsNullOrEmpty(uid.Source))?.Value 
		?? string.Empty;

	public string ResvId =>
		OwsUniqueIds
			.SingleOrDefault(uid => uid.Type == OwsUniqueIdType.INTERNAL && uid.Source == "RESVID")?.Value ?? string.Empty;

	public OwsRoomStayRes? RoomStay { get; set; }
	public List<OwsReservationGuest> Guests { get; set; } = [];
	public OwsReservationHistory? ReservationHistory { get; set; }
	public BookingStatus ReservationStatus { get; set; }
}