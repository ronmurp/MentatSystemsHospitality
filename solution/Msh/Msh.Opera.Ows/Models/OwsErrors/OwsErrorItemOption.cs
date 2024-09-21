namespace Msh.Opera.Ows.Models.OwsErrors;

/// <summary>
/// OwsErrorCodes can have multiple conditions that demand a different error message
/// </summary>
public class OwsErrorItemOption
{
	/// <summary>
	/// A convenient ID. May be used in the trigger conditions. WIP
	/// </summary>
	public string Id { get; set; }

	/// <summary>
	/// Comma delimited set of hotel codes that trigger the condition.
	/// If empty, applies to any hotel. If not empty, applies to only those hotels.
	/// </summary>
	public string Hotels { get; set; }


	/// <summary>
	/// 0 => not used. MinNights > 0 => Used, and if stay nights is less, this condition is triggered 
	/// </summary>
	public int MinNights { get; set; }

	/// <summary>
	/// Comma delimited set of 3-char days of the week, usually consecutive if more than one, but not necessarily.
	/// dow="Sat,Sun"
	/// </summary>
	public string DaysOfWeek { get; set; }


	public DateTime FromDate { get; set; }
	public DateTime ToDate { get; set; }

	public OwsDateRangeType DateType { get; set; }


	/// <summary>
	/// The message to be displayed if the option is triggered
	/// </summary>
	public string Message { get; set; }

}