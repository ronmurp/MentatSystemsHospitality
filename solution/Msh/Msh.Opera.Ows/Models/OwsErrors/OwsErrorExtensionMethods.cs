namespace Msh.Opera.Ows.Models.OwsErrors;

/// <summary>
/// Extension methods to test for various trigger conditions on OwsErrorCodes
/// </summary>
public static class OwsErrorExtensionMethods
{
	public static bool TriggerHotel(this OwsErrorItemOption opt, string hotelCode) =>
		string.IsNullOrEmpty(opt.Hotels) || opt.Hotels.Contains(hotelCode);

	/// <summary>
	/// MinNights is used, and the booking nights is less than
	/// </summary>
	/// <param name="opt"></param>
	/// <param name="nights"></param>
	/// <returns></returns>
	public static bool TriggerMinNights(this OwsErrorItemOption opt, int nights) =>
		opt.MinNights < 1 || nights < opt.MinNights;

	/// <summary>
	/// Dow of Week is used, and the Arrive matches one (of the comma separated "Mon,Tue,...")
	/// </summary>
	/// <param name="opt"></param>
	/// <param name="arrive"></param>
	/// <returns></returns>
	public static bool TriggerDow(this OwsErrorItemOption opt, DateTime arrive) =>
		string.IsNullOrEmpty(opt.DaysOfWeek) || opt.DaysOfWeek.Contains($"{arrive.DayOfWeek}".Substring(0, 3));

	/// <summary>
	/// The stay range, Arrive to Depart, both inclusive, is within the range of FromDate to ToDate 
	/// </summary>
	/// <param name="opt"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <returns></returns>
	public static bool TriggerDates(this OwsErrorItemOption opt, DateTime arrive, DateTime depart) =>
		opt.FromDate == DateTime.MinValue
		|| (arrive >= opt.FromDate && depart <= opt.ToDate && opt.DateType == OwsDateRangeType.Contained)
		|| (opt.FromDate >= arrive && opt.ToDate <= depart && opt.DateType == OwsDateRangeType.Contains)
		|| (arrive >= opt.FromDate && arrive <= opt.ToDate && opt.DateType == OwsDateRangeType.Arrive)
		|| (depart >= opt.FromDate && depart <= opt.ToDate && opt.DateType == OwsDateRangeType.Depart);

}