namespace Msh.Opera.Ows.Models.ReservationResponseModels;

/// <summary>
///  Returned by DecodeOwsPackage in UpdatePackageRequest
/// </summary>
public class OwsPackageExtraRes
{
	public OwsPackageInfo? PackageInfo { get; set; }
	public OwsPackageCharge? ExpectedCharges { get; set; }
}