namespace Msh.Common.Models.BaseModels;

/// <summary>
/// The base address of many address models
/// </summary>
public class BaseAddress
{
	public string AddressLine1 { get; set; } = string.Empty;
	public string AddressLine2 { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string County { get; set; } = string.Empty;
	public string StateProvince { get; set; } = string.Empty;

	/// <summary>
	/// Two character country code
	/// </summary>
	public string CountryCode { get; set; } = "GB";

	/// <summary>
	/// Three character country code
	/// </summary>
	public string CountryCode3 { get; set; } = string.Empty;

	/// <summary>
	/// Numeric character country code
	/// </summary>
	public string CountryCodeN { get; set; } = string.Empty;

	public string PostalCode { get; set; } = string.Empty;

	/// <summary>
	/// In addresses returned from Opera this identifies the address for updates
	/// </summary>
	public string OperaId { get; set; } = string.Empty;

	public bool IsValid(bool isPremierCore = false)
	{
		if (AddressLine1.Trim().Length == 0)
			return false;
		if (City.Trim().Length == 0)
			return false;
		if (!isPremierCore && StateProvince.Trim().Length == 0)
			return false;
		if (PostalCode.Trim().Length == 0)
			return false;
		if (!isPremierCore && CountryCode.Trim().Length == 0)
			return false;

		return true;
	}
}