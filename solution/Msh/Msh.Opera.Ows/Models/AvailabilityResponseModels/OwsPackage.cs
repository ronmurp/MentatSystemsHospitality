namespace Msh.Opera.Ows.Models.AvailabilityResponseModels;

/// <summary>
/// Packages returned by OWS
/// </summary>
public class OwsPackage
{
	public string PackageCode { get; set; } = string.Empty;

	public decimal Amount { get; set; }

	public string CurrencyCode { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;
}