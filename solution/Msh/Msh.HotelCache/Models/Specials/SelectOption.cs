namespace Msh.HotelCache.Models.Specials;

/// <summary>
/// Select options for specials
/// </summary>
public class SelectOption
{
	/// <summary>
	/// Select option value
	/// </summary>
	public string? Value { get; set; } = string.Empty;

	/// <summary>
	/// Select option text
	/// </summary>
	public string? Text { get; set; } = string.Empty;

	/// <summary>
	/// A money value associated with this option
	/// </summary>
	public decimal DataValue { get; set; }
}