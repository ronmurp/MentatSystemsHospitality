namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Used when adding a payment record to a booking (first booking only in a multiple room booking)
/// </summary>
public class OwsAddPaymentRequest : OwsBaseRequest
{
	public string? ReservationId { get; set; }

	public decimal ChargeAmount { get; set; }

	/// <summary>
	/// Description of the purpose of the charge
	/// </summary>
	public string? LongInfo { get; set; }

	public string ShortInfo { get; set; } = string.Empty;

	public DateTime PostDataTime { get; set; }

	public string PostDate => $"{PostDataTime:yyyy-MM-dd}";

	public string PostTime => $"{PostDataTime:HH:mm:ss}";

	/// <summary>
	/// Now need to start picking this up from CreateBooking
	/// </summary>
	public string? ResvId { get; set; }

	/// <summary>
	/// Payment References
	/// </summary>
	public string? Reference { get; set; }

	public string? OwsPaymentCode { get; set; }
}