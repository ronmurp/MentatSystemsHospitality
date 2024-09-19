namespace Msh.Opera.Ows.Models.AvailabilityResponseModels;

/// <summary>
/// Rate plans, as returned by OWS
/// </summary>
public class OwsRatePlan
{
	public string RatePlanCode { get; set; } = string.Empty;
	public string QualifyingIdType { get; set; } = string.Empty;
	public string QualifyingIdValue { get; set; } = string.Empty;
	public string PromotionCode { get; set; } = string.Empty;
}