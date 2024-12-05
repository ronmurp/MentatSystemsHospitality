using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Specials;
using NUnit.Framework;
using System.Xml.Linq;
using Msh.Common.ExtensionMethods;
using Msh.HotelCache.Models;
using Msh.TestSupport;

namespace Msh.Imports.Imports;

[TestFixture]
[Explicit]
public class ImportSpecials
{

	[Test]
	public async Task ImportSpecialsXml()
	{
		var filename = @"C:\Proj2\elh-wbs4\solution\WbsApplication\App_Data\Specials.xml";

		var xdoc = XDocument.Load(filename);

		var list = xdoc.Descendants("Hotel")
			.Select(hs => new ImportHotelSpecials
			{
				HotelCode = hs.ValueA("hotelCode"),
				SpecialsList = hs.Descendants("Specials").Descendants("Special")
					.Select(s => new Special
					{
						Code = s.ValueA("code"),
						Text = s.ValueA("text"),
						ItemType = s.ValueA("CheckBox", "type").Get<SpecialItemType>(),
						
						Enabled = s.ValueA(false, "enabled"),
						IsValued = s.ValueA(false, "isValued"),
						SingleLine = s.ValueA(false, "singleLine"),
						ShortText = s.ValueA("shortText"),
						SelectedText = s.ValueA("selectedText"),
						WarningText = s.ValueE("WarningText"),
						ItemDates = GetItemDates(s),
						Options = GetOptions(s),
						// DisablingSet = GetDisablingSet(s),
						RoomTypeCodes = GetDisablingSet(s).RoomTypeCodes,
						RatePlanCodes = GetDisablingSet(s).RatePlanCodes,
						DisabledText = GetDisablingSet(s).DisabledText,
						AdultsOnly = AdultsOnly(s),
						Notes = $"Imported {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
					}).ToList()
			}).ToList();

		foreach (var hotelSpecials in list)
		{
			await TestConfigUtilities.SaveConfig($"{ConstHotel.Cache.Specials}-{hotelSpecials.HotelCode}", hotelSpecials.SpecialsList);
		}

	}

	public bool AdultsOnly(XElement el)
	{
		var ds = el.Descendants("Disabled")
			.Select(d => new DisablingSet
			{
				AdultsOnly = d.ValueE(false, "AdultsOnly"),
				RoomTypeCodes = d.ValueE("RoomTypeCodes").SplitCommaTrim().ToList(),
				RatePlanCodes = d.ValueE("RatePlanCodes").SplitCommaTrim().ToList(),
				DisabledText = d.ValueE("DisabledText")
			}).SingleOrDefault() ?? new DisablingSet();

		return ds.AdultsOnly;
	}



	public DisablingSet GetDisablingSet(XElement el) =>
		el.Descendants("Disabled")
			.Select(d => new DisablingSet
			{
				AdultsOnly = d.ValueE(false, "AdultsOnly"),
				RoomTypeCodes = d.ValueE("RoomTypeCodes").SplitCommaTrim().ToList(),
				RatePlanCodes = d.ValueE("RatePlanCodes").SplitCommaTrim().ToList(),
				DisabledText = d.ValueE("DisabledText")
			}).SingleOrDefault() ?? new DisablingSet();

	private List<SelectOption> GetOptions(XElement element) =>
		element.Descendants("options").Descendants("option")
			.Select(o => new SelectOption
			{
				Value = o.ValueA("value"),
				Text = o.ValueA("text"),
				DataValue = o.ValueA(0M, "dataValue"),
			}).ToList();

	protected List<ItemDate> GetItemDates(XElement el) =>
		el.Descendants("Date")
			.Select(d => new ItemDate
			{
				DateType = d.ValueA("Prohibit", "dateType").Get<ItemDateType>(),
				Enabled = d.ValueA(true, "enabled"),
				FromDate = d.ValueA(DateOnly.MinValue, "fromDate"),
				ToDate = d.ValueA(DateOnly.MaxValue, "toDate"),
				Notes = d.ValueE("Notes"),
			}).ToList();

}

public class ImportHotelSpecials
{
	public string HotelCode { get; set; } = string.Empty;

	public List<Special> SpecialsList { get; set; } = [];


}