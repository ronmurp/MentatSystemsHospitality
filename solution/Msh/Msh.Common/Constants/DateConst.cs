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

public static partial class CommonConst
{
	public static class Formats
	{
		public const string Date = "dd MMM yyyy"; // Basic date format
	}
}
public static partial class CommonConst
{

	public static class AppSettings
	{
		public const string LogPath = "LogPath";
		public const string MainLogFile = "MainLogFile";
		public const string OwsLogfile = "OwsLogfile";
		public const string TabbedLogFile = "TabbedLogFile";

		public static readonly string SystemLogPathTest = "SystemLogPathTest";
		public static readonly string SystemLogPathLive = "SystemLogPathLive";
	}

	public static class Messages
	{
		public const string UnexpectedError = "An unexpected error has occurred.";

		public const string InvalidWbsCode =
			"Invalid code. Must start with upper, then any upper, lower, digit, hyphen, underscore, but must not end with hyphen or underscore";
	}

	public static class OwsResultStatusFlag
	{
		public static string Success = "SUCCESS";
		public static string Fail = "FAIL";
	}

	public static class OperaErrorCode
	{

		public const string Wbs = "WBS";
		public const string XmlDecode = "XML_DECODE";

		public const string Availability = "AVAIL";
	}
	public static class GdsError
	{
		/// <summary>
		/// Generated in WBS, not from OWS
		/// </summary>
		public static string WbsElementId = "WBS";

		/// <summary>
		/// A WBS code that can't be attributed to anything else
		/// </summary>
		public static string WbsErrorCode = "WBS";
		public const string AvailErrorCode = "AVAIL";


		public const string OwsFault = "OWS_FAULT";
		public const string MissingElement = "MissingElement";

		/// <summary>
		/// A WBS generated error code in Opera base class that could have come from any Opera service
		/// </summary>
		public static string OperaBaseErrorCode = "OPERA_BASE";

		public static string HttpErrorCode = "HTTP";
	}

	public static class AreaOps
	{
		public const string Fit = "FIT";
		public const string Bookings = "BOOKINGS";

		public const string AreaCookieName = "area-ops";
		public const string FitAgentCookieName = "fit-agent";

		public const string AdminUserCookieName = "admin-user";
	}

	public static class SessionKeys
	{

		public const string Trace = "Trace";

		// For the active booking system
		public const string AreaOptions = "AreaOptions";
		public const string FitAgentProfile = "FitAgentProfile";
		public const string FitAgentContactProfile = "FitAgentContactProfile";

		// For admin operations
		public const string TestUser = "TestUser";

		// For Premier Core
		public const string PcCart = "PcCart";
		public const string PcAvailability = "PcAvailability";
		public const string PcReservationHold = "PcReservationHold";
		public const string PcReservationSummary = "PcReservationSummaryVm";
		public const string PcPaymentData = "PcPaymentData";
		public const string PcPaymentSessionKey = "PcPaymentSessionKey";

		public const string PaymentData = "PaymentData";
		public const string PaymentSessionKey = "PaymentSessionKey";
		public const string PcPageSessionKey = "PcPageSessionKey";
		public const string PcPageState = "PcPageState";
		public const string PcQueryString = "PcQueryString";
		public const string AdminDevLogin = "AdminDevLogin";
		public const string LogItStack = "LogItStack";

		public const string PaymentPageType = "PaymentPageType";
	}

	public static class JsonFiles
	{
		public const string EmailList = "EmailList.json";
		public const string EmailConfig = "EmailConfig.json";
	}

	public static class Emails
	{
		/// <summary>
		/// Marks the point in an email outer wrapper where the body should go
		/// </summary>
		public const string EmailBodyMarker = "<!--EMAIL-BODY-->";
	}

}

public static class FitErrorCode
{
	public const string InvalidAgent = "InvalidAgent";
	public const string AgentProblem = "AgentProblem";
	public const string Login = "Login";
	public const string TryAgain = "TryAgain";
	public const string NoAgent = "NoAgent";
	public const string NoAgentCode = "NoAgentCode";
	public const string InvalidAgentPasswordRequest = "InvalidAgentPasswordRequest";
	public const string InvalidAgentPasswordReset = "InvalidAgentPasswordReset";
}

public class TabLog
{
	public const string BKOK = "BKOK";
	public const string BKMOD = "BKMOD";
	public const string PAID = "PAID";
}

public class ObjLog
{
	public const string PcPacSearchComponents = "PcPacSearchComponents";
}