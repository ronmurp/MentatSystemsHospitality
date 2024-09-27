namespace Msh.HotelCache.Models.Companies;

public class CompanyHotel
{
	public string? HotelCode { get; set; }

	/// <summary>
	/// Any room type groups that the company cannot use in a booking.
	/// </summary>
	/// <remarks>
	/// Filters out room types in availability results.
	/// </remarks>
	public List<string> ExcludedRoomTypeGroups { get; set; } = [];

	/// <summary>
	/// If any, these are the rate plans that must be used. For Primary Companies
	/// </summary>
	public List<string> RatePlans { get; set; } = [];
}