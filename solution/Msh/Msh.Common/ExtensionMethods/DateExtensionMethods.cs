using Msh.Common.Models.Dates;

namespace Msh.Common.ExtensionMethods;

public static class DateExtensionMethods
{
    public static List<ItemDate> AddDates(this List<ItemDate> list, List<ItemDate> dates)
    {
        if (dates.Count > 0)
            list.AddRange(dates);
        return list;
    }

    public static string DateString(this DateTime date) => $"{date:yyyy-MM-dd}";

    /// <summary>
    /// Return the last date in the year for the date submitted
    /// </summary>
    public static DateTime YearEndDate(this DateTime date) => new DateTime(date.Year, 12, 31);

    /// <summary>
    /// Return the end of the month for the date submitted.
    /// </summary>
    public static DateTime MonthEndDate(this DateTime date) => new DateTime(date.AddMonths(1).Year, date.AddMonths(1).Month, 1).AddDays(-1);
    
    /// <summary>
    /// Is the submitted date the MinValue (set as a default)
    /// </summary>
    public static bool IsMin(this DateTime dt) => dt.Equals(DateTime.MinValue);

    /// <summary>
    /// Is the submitted date the MaxValue (set as a default)
    /// </summary>
    public static bool IsMax(this DateTime dt) => dt.Equals(DateTime.MaxValue);

    /// <summary>
    /// Is the submitted date the MinValue or the MaxValue
    /// </summary>
    public static bool IsMinMax(this DateTime dt) => IsMin(dt) || IsMax(dt);

    /// <summary>
    /// Number of minutes 
    /// </summary>
    public static int CalculatedMinutes(this object obj, string startHoursMinutes, string endHoursMinutes)
    {
        var today = DateTime.Now.Date;

        var st = today.CombinedDateTime(startHoursMinutes);
        var et = today.CombinedDateTime(endHoursMinutes);

        var minutes = et.Subtract(st).Hours * 60 + et.Subtract(st).Minutes;

        return minutes;
    }

    /// <summary>
    /// Combine a time with a date
    /// </summary>
    public static DateTime CombinedDateTime(this DateTime date, string timeHoursMinutes) =>
        DateTime.Parse($"{date:yyyy-MM-dd}T{timeHoursMinutes}");

    /// <summary>
    /// Only where end == start is OK - i.e. not exclusive
    /// </summary>
    /// <param name="startDateTime1"></param>
    /// <param name="endDateTime1"></param>
    /// <param name="startDateTime2"></param>
    /// <param name="endDateTime2"></param>
    /// <returns></returns>
    public static bool Overlap(this DateTime startDateTime1, DateTime endDateTime1, DateTime startDateTime2, DateTime endDateTime2) =>
        !(endDateTime1 <= startDateTime2 || startDateTime1 >= endDateTime2);


    //public static bool Overlap(this IPcStartEndTimes a, IPcStartEndTimes b) =>
    //    !(a.EndDateTime <= b.StartDateTime || a.StartDateTime >= b.EndDateTime2);

    /// <summary>
    /// Only where end == start is OK - i.e. not exclusive
    /// </summary>
    public static bool NoOverlap(this DateTime startDateTime1, DateTime endDateTime1, DateTime startDateTime2, DateTime endDateTime2) =>
        endDateTime1 <= startDateTime2 || startDateTime1 >= endDateTime2;

    //public static bool NoOverlap(this IPcStartEndTimes a, IPcStartEndTimes b) =>
    //    (a.EndDateTime <= b.StartDateTime || a.StartDateTime >= b.EndDateTime2);

    public static DateTime CoreEndOfDay(this DateTime date) => date.Date.AddDays(1).AddSeconds(-1);

    /// <summary>
    /// Search from whole days
    /// </summary>
    /// <param name="fromDateTimeIn"></param>
    /// <param name="now"></param>
    /// <param name="earliestAllowed"></param>
    /// <returns></returns>
    public static (DateTime fromDateTime, DateTime toDateTime, bool sameDay) ValidActivitiesSearchTimes(this DateTime fromDateTimeIn, DateTime now, DateTime earliestAllowed)
    {
        var fromDateTime = fromDateTimeIn;

        earliestAllowed = new DateTime(earliestAllowed.Year, earliestAllowed.Month, earliestAllowed.Day, earliestAllowed.Hour, 0, 0);
        now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

        // Fix invalid earliest or from
        if (earliestAllowed < now) earliestAllowed = now;
        if (fromDateTime < now) fromDateTime = now;

        // later than earliest so use whole day from
        if (fromDateTime.Date > earliestAllowed.Date) return (fromDateTime.Date, fromDateTime.CoreEndOfDay(), fromDateTime.Date == fromDateTimeIn.Date);

        // Less than earliest but later than today so use earliest whole day
        if (fromDateTime.Date < earliestAllowed && fromDateTime.Date > now.Date) return (earliestAllowed.Date, earliestAllowed.CoreEndOfDay(), earliestAllowed.Date == fromDateTimeIn.Date);

        // if later date than now use from's whole day
        if (fromDateTime.Date > now.Date) return (fromDateTime.Date, fromDateTime.CoreEndOfDay(), fromDateTime.Date == fromDateTimeIn.Date);

        fromDateTime = new DateTime(fromDateTime.Year, fromDateTime.Month, fromDateTime.Day, fromDateTime.Hour, 0, 0);

        // Can't search 
        if (fromDateTime < earliestAllowed) fromDateTime = earliestAllowed;

        return (fromDateTime, fromDateTime.CoreEndOfDay(), fromDateTime.Date == fromDateTimeIn.Date);

    }
    // From   Now   Early
    // 10th   9th   10th => 10 - 11
    // 10th   9th   12th => 12 - 13
    //

    public static bool HasBookDates(this HotelDateItem hotelDateItem) => 
	    hotelDateItem.BookFrom != DateOnly.MinValue && hotelDateItem.BookTo != DateOnly.MinValue;

	/// <summary>
	/// The hotel is disabled
	/// </summary>
	/// <param name="hotelDateItem"></param>
	/// <param name="arrive"></param>
	/// <param name="depart"></param>
	/// <returns>Returns true if disabled</returns>
	/// <remarks>
	/// 
	/// |------|                |----| OK
	///         |- IsDisabled -|
	///       |----| |---|  |-----| disable
	///       |-------------------| disable
	/// </remarks>
	public static bool IsDisabled(this HotelDateItem hotelDateItem, DateOnly arrive, DateOnly depart) =>
	    !(depart <= hotelDateItem.StayFrom || arrive > hotelDateItem.StayTo);

    /// <summary>
    /// True if the date is within the date range
    /// </summary>
    /// <param name="hotelDateItem"></param>
    /// <param name="date"></param>
    /// <returns></returns>
	public static bool IsDisabled(this HotelDateItem hotelDateItem, DateOnly date) =>
		date >= hotelDateItem.StayFrom && date <= hotelDateItem.StayTo;

    /// <summary>
    /// Cannot book the hotel if the 
    /// </summary>
    /// <param name="hotelDateItem"></param>
    /// <param name="now"></param>
    /// <returns></returns>
    public static bool CannotBook(this HotelDateItem hotelDateItem, DateOnly now) =>
	    now >= hotelDateItem.StayFrom && now <= hotelDateItem.StayTo;
}