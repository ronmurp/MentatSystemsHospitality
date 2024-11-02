using Msh.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Msh.HotelCache.Models.RatePlans;

/// <summary>
/// A list of common texts that can be used in rate plans and updated once rather than for every rate plan.
/// </summary>
public class RatePlanText
{
	[Required]
	[Display(Name = "Id"), Info("id")]
	public string Id { get; set; } = string.Empty;

	[Required]
	[Display(Name = "Text"), Info("text")]
	public string Text { get; set; } = string.Empty;

	[Required]
	[Display(Name = "Notes"), Info("notes")]
	public string Notes { get; set; } = string.Empty;
}