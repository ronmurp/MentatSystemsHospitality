namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsGuestCount
{
	public OwsAgeQualifyingCode AgeQualifyingCode { get; set; }
	public int Count { get; set; }
}