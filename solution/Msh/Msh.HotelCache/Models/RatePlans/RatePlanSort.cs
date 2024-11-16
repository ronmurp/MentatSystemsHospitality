using Msh.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Msh.HotelCache.Models.RatePlans;

/// <summary>
/// A list of these is used to determine the order of presentation of rate plans to the customer.
/// If no sort order is provided, the rate plans will be presented in the default order.
/// Any rate plan codes not included here will be listed at the end of the ordered list, in the default order.
/// </summary>
public class RatePlanSort
{
	[Required]
	[Display(Name = "Code"), Info("info-code")]
	public string Code { get; set; } = string.Empty;

	/// <summary>
	/// A numeric value representing the order - lowest first - i.e. ascending.
	/// </summary>

	[Display(Name = "Order"), Info("info-order")]
	public int Order { get; set; }
}