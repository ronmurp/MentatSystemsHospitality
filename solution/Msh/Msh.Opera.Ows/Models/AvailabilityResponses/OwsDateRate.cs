using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OwsDateRate
{
	public bool RateChangeIndicator { get; set; }
	public DateTime EffectiveDate { get; set; }
	public SoapAmount? Amount { get; set; }
}