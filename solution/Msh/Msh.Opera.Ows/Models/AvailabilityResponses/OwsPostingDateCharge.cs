namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OwsPostingDateCharge
{
	public DateTime PostingDate { get; set; }
	public OwsCharges? RoomRateAndPackages { get; set; }
	public OwsCharges? TaxesAndFees { get; set; }
}