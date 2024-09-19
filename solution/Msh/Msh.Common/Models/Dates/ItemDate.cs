using System.Text.Json.Serialization;

namespace Msh.Common.Models.Dates;

public class ItemDate
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public ItemDateType DateType { get; set; } = ItemDateType.Prohibit;
	public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public bool Enabled { get; set; } = true;
    public string Notes { get; set; } = string.Empty;

}