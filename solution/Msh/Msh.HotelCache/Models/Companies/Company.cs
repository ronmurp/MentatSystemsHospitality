namespace Msh.HotelCache.Models.Companies;

/// <summary>
/// A Company that can make bookings. From the Companies.xml files
/// </summary>
public class CompanyConfig
{
	/// <summary>
	/// Corresponds to the Opera Profile ID for the company
	/// </summary>
	public string? ClientId { get; set; }
	public CompanyType CompanyType { get; set; }
	public string? Name { get; set; }
	public string? Email { get; set; }

	public string? IATA { get; set; }
	public bool Enabled { get; set; }
	public string? Sourcecode { get; set; }
	public string? MarketSegment { get; set; }
	public CompanyAuthMethod AuthMethod { get; set; }
	public bool IsDbb { get; set; }

	public string Language { get; set; } = "en";

	public PayOptions PayOption { get; set; } = PayOptions.ByCredit;

	/// <summary>
	/// PayInFull, if true, overrides any deposit that might be calculated, IFF ByCredit is used.
	/// </summary>
	public bool PayInFull { get; set; }

	/// <summary>
	/// Applies only for CompanyType.Standard
	/// </summary>
	public decimal Discount { get; set; }

	public List<CompanyHotel> Hotels { get; set; } = [];

	public List<CompanyAgent> Agents { get; set; } = [];

	public string? Notes { get; set; }
}