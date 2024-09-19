namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
/// <remarks>
/// See also OwsPhone
/// </remarks>
public class OwsEmail
{
	public bool Primary { get; set; }
	public string? EmailType { get; set; }
	public string? Email { get; set; }
}