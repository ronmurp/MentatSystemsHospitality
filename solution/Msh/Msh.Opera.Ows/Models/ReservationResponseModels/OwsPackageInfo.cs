using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
///  Returned by DecodeOwsPackage in UpdatePackageRequest
/// </summary>
public class OwsPackageInfo
{
	public string?  PackageCode { get; set; }
	public string?  CalculationRule { get; set; }
	public string?  PostingRhythm { get; set; }
	public bool TaxIncluded { get; set; }
	public SoapAmount? Amount { get; set; }
	public string?  Description { get; set; }
	public string?  ShortDescription { get; set; }
}