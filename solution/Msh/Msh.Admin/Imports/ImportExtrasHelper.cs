using System.Xml.Linq;
using Msh.Common.Data;
using Msh.Common.Models.Dates;
using Msh.HotelCache.Models.Extras;

namespace Msh.Admin.Imports;


public static class ImportExtrasHelper
{
	public static async Task<List<ExtrasContainer>> ImportExtrasXml(IConfigRepository repo, string filename)
	{
		var xml = await File.ReadAllTextAsync(filename);

		var xdoc = XDocument.Parse(xml);

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

		return list;
	}

	public static async Task<List<Extra>> ImportExtrasHotelXml(IConfigRepository repo, string filename, string hotelCode)
	{
		var list = await ImportExtrasXml(repo, filename);

		return list.FirstOrDefault(h => h.HotelCode == hotelCode)?.Extras ?? [];
	}
}

public class ExtrasContainer
{
	public string HotelCode { get; set; } = string.Empty;

	public List<Extra> Extras { get; set; } = [];
}