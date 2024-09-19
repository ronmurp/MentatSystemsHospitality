using Msh.Common.Models.OwsCommon;

namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
///  Returned by DecodeOwsPackage in UpdatePackageRequest
/// </summary>
public class OwsPackageCharge
{
	public string? PackageCode { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public SoapAmount? UnitAmount { get; set; }
	public int Quantity { get; set; }
	public SoapAmount? Tax { get; set; }
	public SoapAmount? Total { get; set; }
}