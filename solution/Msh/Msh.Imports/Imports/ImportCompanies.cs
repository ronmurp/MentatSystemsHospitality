using System.Xml.Linq;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Companies;
using Msh.HotelCache.Models.Greens;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportCompanies
{
	[Test]
	public async Task ImportCompaniesXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Companies\Companies.xml";

		var xdoc = XDocument.Load(filename);

		var list = xdoc.Descendants("Company")
			.Select(c => new CompanyConfig
			{

				ClientId = c.ValueA("id"),
				Enabled = c.ValueA(false, "enabled"),
				Sourcecode = c.ValueA("sourceCode"),
				MarketSegment = c.ValueA("marketSegment"),

				Name = c.ValueE("Name"),
				IATA = c.ValueE("Iata"),
				CompanyType = c.ValueE("CompanyType").Get<CompanyType>(),
				Language = c.ValueE("en", "Language").ToLower(),
				Email = c.ValueE("CompanyEmail"),
				AuthMethod = c.ValueE("AuthMethod").Get<CompanyAuthMethod>(),
				PayOption = c.ValueE("PaymentOption").Get<PayOptions>(),
				PayInFull = c.ValueE(false, "FullPrePay"),
				Discount = c.ValueE(0M, "Discount"),
				Hotels = c.Descendants("Hotel")
					.Select(h => new CompanyHotel
					{
						HotelCode = h.ValueA("id"),
						RatePlans = h.ValueA("ratePlans").SplitCommaTrimUpper().ToList()
					}).ToList(),
				Agents = c.Descendants("Agent")
					.Select(a => new CompanyAgent
					{
						ProfileId = a.ValueA("id"),
						Name = a.ValueE("AgentName"),
						Email = a.ValueE("AgentEmail"),
						AuthMethod = c.ValueE("AgentAuthMethod").Get<CompanyAuthMethod>(),
					}).ToList()

			}).ToList();

		await TestConfigUtilities.SaveConfig(ConstHotel.Cache.Companies, list);

	}
}