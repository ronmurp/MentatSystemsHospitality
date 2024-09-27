namespace Msh.HotelCache.Models.Countries;

public class CountriesConfig
{
	public string DefaultCountryCode { get; set; } = "UK";

	public bool IsoLimit { get; set; } = true;

	public string? Notes { get; set; }
}