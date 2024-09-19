using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OwsExpectedCharges
{
	public SoapAmount? TotalRoomRateAndPackages { get; set; }
	public SoapAmount? TotalTaxesAndFees { get; set; }
	public bool TaxInclusive { get; set; }
	public List<OwsPostingDateCharge> DayCharges { get; set; } = [];

}