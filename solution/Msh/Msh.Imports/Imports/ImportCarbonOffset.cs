using System.Xml;
using System.Xml.Linq;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Greens;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportCarbonOffset
{
	[Test]
	public async Task ImportCarbonOffsetXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\CarbonOffset\CarbonOffset-Live.xml";

		var xdoc = XDocument.Load(filename);

		var carbonOffset = xdoc.Descendants("data")
			.Select(c => new CarbonOffset
			{
				Enabled = c.ValueE(false, "Enabled"),
				DefaultPay = c.ValueE(false, "DefaultPay"),
				Percent = c.ValueE(0M, "Percent"),
				CheckboxPromptHtml = c.ValueE("CheckboxPrompt"),
				ConfirmationHtml = c.ValueE("ConfirmationText"),
				MinimumCharge = c.ValueE(0M, "MinimumCharge"),
				OperaAcceptedText = c.ValueE("OperaAcceptedText"),
				OperaDeclinedText = c.ValueE("OperaDeclinedText"),
				Notes = $"Imported {DateTime.Now:yyyy-MM-ddTHH-mm-ss}"

			}).SingleOrDefault() ?? new CarbonOffset();

		await TestConfigUtilities.SaveConfig(ConstHotel.Cache.CarbonOffset, carbonOffset);

	}
}