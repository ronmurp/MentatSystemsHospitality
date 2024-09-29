﻿using System.Xml.Linq;
using Msh.Common.Models.Dates;
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
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Extras\Extras-Live.xml";

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
						ItemDates = x.Descendants("Date")
							.Select(d => new ItemDate
							{
								DateType = ItemDateType.Allow,
								Enabled = d.ValueA(true, "enabled"),
								FromDate = d.ValueA(DateOnly.MinValue, "fromDate"),
								ToDate = d.ValueA(DateOnly.MaxValue, "toDate"),
								Notes = d.ValueE("Notes"),
							}).ToList()

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