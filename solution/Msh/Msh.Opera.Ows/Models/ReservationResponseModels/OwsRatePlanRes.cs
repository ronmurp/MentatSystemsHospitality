using Msh.Opera.Ows.Models.AvailabilityResponseModels;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsRatePlanRes : OwsRatePlan
{
	public bool SuppressRate { get; set; }
	public string? RatePlanName { get; set; }

	/// <summary>
	/// A description returned by OWS - not necessarily the same as the WBS description
	/// </summary>
	public string? Description { get; set; }
}