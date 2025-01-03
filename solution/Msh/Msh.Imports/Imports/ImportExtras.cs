﻿using Msh.Admin.Imports;
using Msh.HotelCache.Models;
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

		var list = await ImportExtrasHelper.ImportExtrasXml(TestConfigUtilities.GetRepository(), filename);

		foreach (var ec in list)
		{

			await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.Extras}-{ec.HotelCode}", ec.Extras);
		}
	}
}