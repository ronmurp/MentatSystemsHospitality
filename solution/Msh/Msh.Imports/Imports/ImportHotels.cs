using System.Xml.Linq;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportHotels
{
	[Test]
	public async Task ImportHotelsXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\HotelList\HotelList.xml";

		var xdoc = XDocument.Load(filename);

		var list = xdoc.Descendants("Hotel")
			.Select(h => new Hotel
			{
				ChainCode = h.ValueA("CHA", "chainCode"),
				Enabled = h.ValueA(true, "enabled"),
				RoomTypeGroups = h.ValueA(true, "useRoomTypeGroups"),
				HotelCode = h.ValueA("hotelCode"),
				CrsNumber = h.ValueA("crsnumber"),
				CrsNumberInt = h.ValueA("crsNumberInt"),
				Name = h.ValueA("name"),
				ImageFile = h.ValueA("imageFile"),
				SearchImageFile = h.ValueA("searchImageFile"),
				Subtitle = h.ValueE("subtitle"),
				Description = "",//ElementOrFile(h.Descendants("description").FirstOrDefault()),// h.ValueE("description"),
				ConfirmCopy = h.ValueE("ConfirmCopy"),
				// HotelDateList = GetHotelDates(h),
				DisabledText = h.ValueE("DisabledText"),
				NoDogs = h.ValueE(false, "NoDogs"),
				NoDogsText = h.ValueE("NoDogsText")
			}).ToList();

		await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.Hotel}", list);


	}


}