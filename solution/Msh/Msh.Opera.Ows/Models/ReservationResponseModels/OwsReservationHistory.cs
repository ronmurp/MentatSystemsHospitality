namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsReservationHistory
{
	public string? InsertUser { get; set; }
	public DateTime InsertDate { get; set; }
	public string? UpdateUser { get; set; }
	public DateTime UpdateDate { get; set; }
}