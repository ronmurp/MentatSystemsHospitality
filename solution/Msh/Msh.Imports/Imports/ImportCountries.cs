using System.Xml.Linq;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Countries;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportCountries
{
	[Test]
	public async Task ImportCountriesIsoXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\CountryCodes-ISO-3166.xml";

		var xdoc = XDocument.Load(filename);

		var countries = xdoc.Descendants("country")
			.Select(v => new Country
			{
				Code = v.ValueA("code"),
				Name = v.ValueE("Name"),
				Code3 = v.ValueA("code3"),
				CodeN = v.ValueA("codeN")
			}).ToList();

		await TestConfigUtilities.SaveConfig(ConstHotel.Cache.CountriesIso, countries);

	}

	[Test]
	public async Task ImportCountriesXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Countries.xml";

		var xdoc = XDocument.Load(filename);

		var countries = xdoc.Descendants("country")
			.Select(v => new Country
			{
				Code = v.ValueA("code"),
				Name = v.ValueA("name")
			}).ToList();

		await TestConfigUtilities.SaveConfig(ConstHotel.Cache.Countries, countries);

	}

	[Test]
	public async Task ImportCountriesConfig()
	{
		var config = new CountriesConfig
		{
			Notes = ""
		};

		await TestConfigUtilities.SaveConfig(ConstHotel.Cache.CountriesConfig, config);

	}
}