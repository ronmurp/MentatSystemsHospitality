namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Used for booking extras in the booking rather than UpdateExtras separate call
/// </summary>
public class OwsReservationExtra
{
	public string? PackageCode { get; set; }
	public int Quantity { get; set; } = 1;
}