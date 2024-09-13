namespace Msh.Common.ExtensionMethods;

public static class DateOnlyExtensionMethods
{
	public static string DateString(this DateOnly date) => $"{date:yyyy-MM-dd}";

	/// <summary>
	/// Is the submitted date the MinValue (set as a default)
	/// </summary>
	public static bool IsMin(this DateOnly dt) => dt.Equals(DateOnly.MinValue);

	/// <summary>
	/// Is the submitted date the MaxValue (set as a default)
	/// </summary>
	public static bool IsMax(this DateOnly dt) => dt.Equals(DateOnly.MaxValue);

	/// <summary>
	/// Is the submitted date the MinValue or the MaxValue
	/// </summary>
	public static bool IsMinMax(this DateOnly dt) => IsMin(dt) || IsMax(dt);


	/// <summary>
	/// Return the last date in the year for the date submitted
	/// </summary>
	public static DateOnly YearEndDate(this DateOnly date) => new DateOnly(date.Year, 12, 31);

	/// <summary>
	/// Return the end of the month for the date submitted.
	/// </summary>
	public static DateOnly MonthEndDate(this DateOnly date) => new DateOnly(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1);

}