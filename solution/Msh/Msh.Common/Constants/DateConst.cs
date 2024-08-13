namespace Msh.Common.Constants;

public static class DateConst
{


    /// <summary>
    /// Color classes for identifying date conditions
    /// </summary>
    public const string SpanError = "error-date";       // red
    public const string SpanActive = "active-date";     // blue
    public const string SpanInActive = "inactive-date";     // gray
    public const string SpanFuture = "future-date";     // green
    public const string SpanMinMax = "min-max-date";    // gray
    public const string SpanNormal = "";                // No class, normal color
    public const string SpanPast = "past-date";         // Date is in the past

    public const string FromAfterTo = "The From date is equal to or greater than To date. ERROR.";
    public const string StayToPast = "The Stay To date is in the past. No longer active.";
    public const string StayActive = "The Stay date range is active.";
    public const string StayFuture = "The Stay date range is in the future.";

    public const string BookTooLate = "The booking dates are later than the stay dates. ERROR.";
    public const string BookToIsLate = "The Book To date is later than the Stay To date.";
    public const string BookToPast = "The Book To date is in the past. No longer active.";

    public const string DateFormat = "yyyy-MM-dd";
}