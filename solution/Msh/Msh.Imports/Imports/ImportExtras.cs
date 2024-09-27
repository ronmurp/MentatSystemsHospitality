using System.Xml.Linq;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportExtras
{
	[Test]
	public async Task ImportExtrasXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Extras\Extras-Test.xml";

		var xdoc = XDocument.Load(filename);

		var list = xdoc.Descendants("Hotel")
			.Select(e => new ExtrasContainer
			{
				HotelCode = e.ValueA("hotelCode"),
				Extras = e.Descendants("Extra")
					.Select(x => new Extra
					{
						Code = x.ValueA("code"),
						Enabled = x.ValueA(false, "enabled"),
						Price = x.ValueA(0M, "price"),
						DisplayText = x.ValueA("displayText"),

					}).ToList()

			}).ToList();

		foreach (var ec in list)
		{

			await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.Extras}-{ec.HotelCode}", ec.Extras);
		}

	}

	
}

public class ExtrasContainer
{
	public string HotelCode { get; set; } = string.Empty;

	public List<Extra> Extras { get; set; } = [];
}