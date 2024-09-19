using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.AvailabilityResponses;

/// <summary>
/// Returned in a detailed availability request
/// </summary>
public class OperaCharge
{
	public string? Description { get; set; }
       
	public string? CodeType { get; set; }
	public string? Code { get; set; }
	public SoapAmount? Amount { get; set; }
}