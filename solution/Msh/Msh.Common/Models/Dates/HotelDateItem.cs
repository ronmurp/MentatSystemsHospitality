namespace Msh.Common.Models.Dates;

/// <summary>
/// Dates within which a hotel cannot be booked, or within which a guest cannot stay
/// </summary>
public class HotelDateItem
{
	/// <summary>
	/// StayFrom -> StayTo - The date range within which users cannot stay
	/// </summary>
	public DateOnly StayFrom { get; set; } = DateOnly.MinValue;
	public DateOnly StayTo { get; set; } = DateOnly.MinValue;

	/// <summary>
	/// BookFrom -> BookFrom - The date range within which users cannot make the booking
	/// </summary>
	public DateOnly BookFrom { get; set; } = DateOnly.MinValue;
	public DateOnly BookTo { get; set; } = DateOnly.MinValue;

	public bool StayEnabled { get; set; }

	public bool BookEnabled { get; set; }
}