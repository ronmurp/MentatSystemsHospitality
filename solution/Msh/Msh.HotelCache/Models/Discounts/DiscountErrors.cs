using System.Text.Json.Serialization;

namespace Msh.HotelCache.Models.Discounts;

public static class DiscountErrorsHelper
{
	public static List<string> GetErrorTypes()
	{
		var list = new List<string>();
		foreach (DiscountErrorType et in (DiscountErrorType[])Enum.GetValues(typeof(DiscountErrorType)))
		{
			list.Add($"{et}");
		}

		return list;
	}

	// DiscountErrorsHelper.GetErrorTypes().Select(e => new AdminSelectItem { Value = e, Text = e }).ToList()
}

/// <summary>
/// 
/// </summary>
public class DiscountError
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public DiscountErrorType ErrorType { get; set; }
	public string? Text { get; set; }
}