using Msh.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Msh.Common.Models.Dates;

namespace Msh.HotelCache.Models.Extras;

/// <summary>
/// Copy of an Opera Package
/// </summary>
public class Extra
{
	[Required]
	[Description("Extra Code"), Info("extra-code")]
	public string Code { get; set; } = string.Empty;


	[DataType(DataType.Currency)]
	[Description("Extra Price"), Info("extra-price")]
	public decimal Price { get; set; }


	[Description("Display Text"), Info("extra-display-text")]
	public string? DisplayText { get; set; } = string.Empty;


	[Description("Updated"), Info("extra-updated")]
	public bool Updated { get; set; }


	[Description("Enabled"), Info("extra-enable")]
	public bool Enabled { get; set; }


	[Description("Enabled All"), Info("extra-enable-all")]
	public bool EnabledAll { get; set; }


	[Description("Item Dates"), Info("extra-dates")]
	public List<ItemDate> ItemDates { get; set; } = [];


	[Description("Notes"), Info("extra-notes")]
	public string? Notes { get; set; } = string.Empty;
}