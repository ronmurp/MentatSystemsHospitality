namespace Msh.Opera.Ows.Models.OwsErrors;

/// <summary>
/// Options for how date ranges are used in OwsErrorCodes.xml dateType
/// </summary>
public enum OwsDateRangeType
{
	/// <summary>
	/// Arrive and Depart are contained within the date range
	/// </summary>
	Contained,

	/// <summary>
	/// Arrive and Depart contain the date range
	/// </summary>
	Contains,

	/// <summary>
	/// Arrive is within the date range
	/// </summary>
	Arrive,

	/// <summary>
	/// Depart is within the date range
	/// </summary>
	Depart
}