namespace Msh.Common.Exceptions;

/// <summary>
/// Converts a GDSError into a set of properties used in OwsError lookups
/// </summary>
/// <remarks>
/// Example:
/// <Result resultStatusFlag="FAIL">
///   <hc:GDSError errorCode="SYS" elementId="81">INVALIDGDS</hc:GDSError>
/// </Result>
///
/// </remarks>
public class LibException : Exception
{
	public LibException(string message, string methodInfo) :
		base(message)
	{
		MethodInfo = methodInfo;
	}


	/// <summary>
	/// The method that threw the error - also see base class Source
	/// </summary>
	public string MethodInfo { get; set; }

	/// <summary>
	/// GDSError.elementId, mapped to OwsError.Prefix: prefix="PID" suffix="10" type="U"
	/// </summary>
	public string ErrorCodePrefix { get; set; } = string.Empty;

	/// <summary>
	/// GDSError.errorCode
	/// </summary>
	public string ErrorCodeSuffix { get; set; } = string.Empty;

	/// <summary>
	/// OperaErrorCode
	/// </summary>
	public string OperaErrorCode { get; set; } = string.Empty;

	/// <summary>
	/// Mapped from the OwsError.ErrorType in prefix="PID" suffix="10" type="U"
	/// </summary>
	public string ErrorType { get; set; } = "0";

	public string AdditionalText { get; set; } = string.Empty;
	public string ErrorValue { get; set; } = string.Empty;


	public static void ThrowNew(Exception ex, string methodInfo)
	{
		var message = ex.Message;
		var exi = ex.InnerException;
		while (exi != null)
		{
			message += Environment.NewLine + exi.Message;
			exi = exi.InnerException;
		}
		var exNew = new LibException(message, methodInfo);
		if (ex.GetType() == typeof(LibException))
		{
			var lEx = ((LibException)(ex));
			if (lEx.ErrorCodePrefix.Length > 0 || lEx.ErrorCodeSuffix.Length > 0 || lEx.OperaErrorCode.Length > 0 || lEx.ErrorType.Length > 0)
			{
				exNew.ErrorCodePrefix = lEx.ErrorCodePrefix;
				exNew.ErrorCodeSuffix = lEx.ErrorCodeSuffix;
				exNew.OperaErrorCode = lEx.OperaErrorCode;
				exNew.ErrorType = lEx.ErrorType;
			}
		}

		try
		{
			//WbsLogger.Error(LogCodes.OwsLibEx, exNew);
		}
		catch
		{
		}
		throw exNew;
	}

	private delegate string GetMessage(string prefix, string suffix, string operaErrorCode);
	//public string UserErrorMessage() => 
	//    TypedCache.Instance.OwsErrorList
	//        .GetMessage(ErrorCodePrefix, ErrorCodeSuffix, ErrorCodeText);
	//public override string ToString() => UserErrorMessage();

	public string SummaryText()
	{
		return ($"T={ErrorType}, P={ErrorCodePrefix}, S={ErrorCodeSuffix}, M={OperaErrorCode}");
	}


	public void LogError()
	{
		try
		{
			//WbsLogger.Info(LogCodes.OwsError, this);
		}
		catch //(Exception ex)
		{
			// ignored
		}
	}
}