using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
/// Returned by OWS reservation as part of OwsReservationResponse
/// </summary>
public class OwsPackageRes
{
	public string? Source { get; set; }
	public bool TaxIncluded { get; set; }
	public SoapAmount? PackageAmount { get; set; }
	public SoapAmount? TaxAmount { get; set; }
	public SoapAmount? Allowance { get; set; }
	public string? PackageCode { get; set; }
}