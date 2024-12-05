using System.Xml.Linq;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportRoomTypes
{
	[Test]
	public async Task ImportRoomTypesLists()
	{

		var filename = @$"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\RoomTypes.xml";

		var xdoc = XDocument.Load(filename);

		var list = xdoc.Descendants("Hotel")
			.Select(h => new ImportRatePlans.HotelRoomTypes
			{
				HotelCode = h.ValueA("hotelCode"),
				UseRoomTypeGroups = h.ValueA(false, "useRoomTypeGroups"),
				RoomTypeList = h.Descendants("RoomType")
					.Select(r => new RoomType
					{
						Code = r.ValueA("code"),
						GroupCode = r.ValueA("groupCode"),

						RoomCategory = r.ValueA("roomCategory"),
						RoomCount = r.ValueA(0, "roomCount"),
						RoomBlocked = r.ValueA(false, "blocked"),

						Name = r.ValueA("description"),

						ShortDescription = r.ValueA("shortDescription"),

						ImageFile = r.ValueA("imageFile"),

						Occupancy = r.ValueA(0, "occupancy"),

						TwinOption = r.ValueA(false, "twinOption"),

						OptionTypeCode = r.ValueA("optionTypeCode"),
						OptionTypeWarning = r.ValueA("optionTypeWarning"),

						HtmlDescription = r.ValueE("longDescription"),

						ImageGallery = r.ValueE("ImageFileGallery"),

						NoDogs = r.ValueE(false, "NoDogs"),
						NoDogsText = r.ValueE("NoDogsText")

					}).ToList()
			}).ToList();

		foreach (var rtl in list)
		{
			await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.RoomTypes}-{rtl.HotelCode}", rtl.RoomTypeList);
		}

	}
}