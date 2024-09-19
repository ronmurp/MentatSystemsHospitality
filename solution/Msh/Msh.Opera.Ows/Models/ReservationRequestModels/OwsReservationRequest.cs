using Msh.Opera.Ows.Models.ReservationResponseModels;

namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Based on OwsAvailabilityRequest, adds additional properties for use in Create/Modify Booking
/// </summary>
public class OwsReservationRequest : OwsAvailabilityRequest
{
	public List<OwsUniqueId> UniqueIdList { get; set; } = [];
	public List<OwsReservationGuest> Guests { get; set; } = [];
	public string? QualifyingIdType { get; set; }
	public string? QualifyingIdValue { get; set; }
	public string? PromotionCode { get; set; }
	public List<OwsComment> Comments { get; set; } = [];
	public bool Modify { get; set; }
	public OwsReservationActionType ReservationAction { get; set; }
	public int LegNumber { get; set; }
	public string? ReservationId { get; set; }

	public List<OwsReservationExtra> Extras { get; set; } = [];
	public string AgentUserProfileId { get; set; } = string.Empty;
}