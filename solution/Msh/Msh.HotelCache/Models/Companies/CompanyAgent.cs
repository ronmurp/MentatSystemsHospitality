namespace Msh.HotelCache.Models.Companies;

/// <summary>
/// A company agent or employee - a person that can make bookings on behalf of a company.
/// </summary>
public class CompanyAgent
{
	public string? Email { get; set; }

	/// <summary>
	/// Opera Profile ID
	/// </summary>
	public string? ProfileId { get; set; }

	public string? Name { get; set; }

	public CompanyAuthMethod AuthMethod { get; set; }
}