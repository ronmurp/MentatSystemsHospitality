namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
/// <remarks>
/// Can be an email. Compare with OwsEmail
/// </remarks>
public class OwsPhone
{
	public string?  PhoneType { get; set; }
	public string?  PhoneRole { get; set; }
	public bool Primary { get; set; }
	public string?  Telephone { get; set; }
}