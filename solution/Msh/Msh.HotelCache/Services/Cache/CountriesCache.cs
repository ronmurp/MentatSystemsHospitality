using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Countries;

namespace Msh.HotelCache.Services.Cache;

public interface ICountriesCacheService
{
	Task<List<Country>> GetCountries();

	void ReloadCountries();
}

/// <summary>
/// Return (or reload) Hotel
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public class CountriesCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
	: DataCacheService(memoryCache, configRepository), ICountriesCacheService
{
	public async Task<List<Country>> GetCountries() =>
		await base.GetData<List<Country>>(ConstHotel.Cache.Countries);

	public void ReloadCountries() => base.Reload(ConstHotel.Cache.Countries);

	// Todo - Countries - the cache must run this when reloading. Only the final output countries are required.
	private List<Country> BuildOutputCountries(List<Country> countries, List<Country> isoList, bool owsLimit)
	{
		var list = new List<Country>();

		if (isoList.Count > 0 && owsLimit)
		{
			// These are used on OWS but are not ISO-3166 Code values. Map to ISO-3166
			var lookup = new Dictionary<string, Country>
			{
				{"ENG", new Country{Code = "GB", Code3 = "GBR", CodeN = "826", Name = "England" }},
				{"SCO", new Country{Code = "GB", Code3 = "GBR", CodeN = "826", Name = "Scotland" }},
				{"UK", new Country{Code = "GB", Code3 = "GBR", CodeN = "826", Name = "United Kingdom" }},
				{"WAL", new Country{Code = "GB", Code3 = "GBR", CodeN = "826", Name = "Wales" }},
				{"RQ", new Country{Code = "RQ", Code3 = "RUS", CodeN = "643", Name = "Russian Federation" } },
				{"IRL", new Country{Code = "IRL", Code3 = "IRL", CodeN = "372", Name = "Ireland" }  }
			};

			// The final list is going to be limited to the OWS list, with Code3 and CodeN merely updated from the isoList
			foreach (var country in countries)
			{
				var iso = isoList.FirstOrDefault(i => i.Code.Equals(country.Code, StringComparison.OrdinalIgnoreCase));

				if (iso != null)
				{
					// isoList has a Code that matches country's code, so update from iso
					country.Code3 = iso.Code3;
					country.CodeN = iso.CodeN;
				}
				else
				{
					// No matching iso, so is it an explicit lookup?
					var lu = lookup.FirstOrDefault(l => l.Key.Equals(country.Code, StringComparison.OrdinalIgnoreCase)).Value;
					if (lu != null)
					{
						// map explicit know iso Code3 to OWS specific codes
						country.Code3 = lu.Code3;
						country.CodeN = lu.CodeN;
					}
				}
			}
		}

		// If no iso codes, or iso codes but owsLimit, then use OWS countries (whether mapped Code3 is available or not)
		list.AddRange(isoList.Count == 0 || owsLimit ? countries : isoList);

		// Todo - Deal with the default country 
		//md.DefaultCountryCode = defaultCountry?.DefaultCountry ?? "UK";

		return list;
	}
}