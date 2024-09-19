using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.AvailabilityResponses;

public class OwsCharges
{
	public OperaChargeTypes ChargesType { get; set; }
	public SoapAmount? TotalCharges { get; set; }
	public List<OperaCharge> Charges { get; set; } = [];
}