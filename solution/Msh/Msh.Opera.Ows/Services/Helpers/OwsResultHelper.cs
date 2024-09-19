using Msh.Common.Constants;
using Msh.Common.Exceptions;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Helpers;

/// <summary>
/// Returns specific OwsResult objects
/// </summary>
public static class OwsResultHelper
{
	/// <summary>
	/// A generic OwsResult created in the OwsResult pattern for unexpected results
	/// </summary>
	public static OwsResult WbsResult =>
		new OwsResult()
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = CommonConst.Messages.UnexpectedError,
			OperaErrorCode = CommonConst.OperaErrorCode.Wbs,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = CommonConst.GdsError.WbsErrorCode
			}
		};

	/// <summary>
	/// A generic OwsResult created in the OwsResult pattern for unexpected results
	/// </summary>
	public static OwsResult WbsResultMessage(string errorCode, string message) =>
		new OwsResult()
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = message,
			OperaErrorCode = CommonConst.OperaErrorCode.Wbs,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = errorCode
			}
		};

	public static OwsResult GetWbsResult(string errorCode, string operaErrorCode) =>
		new OwsResult()
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = CommonConst.Messages.UnexpectedError,
			OperaErrorCode = operaErrorCode,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = errorCode
			}
		};

	/// <summary>
	/// OWS has returned a Fault element. Usually something wrong with the sent message. Timestamp out of date, for example.
	/// </summary>
	/// <param name="faultCode"></param>
	/// <param name="faultSource">Client/Server</param>
	/// <param name="faultMessage"></param>
	/// <returns></returns>
	public static OwsResult FaultResult(string faultCode, string faultSource, string faultMessage, string faultValue = "") =>
		new OwsResult()
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = faultMessage,
			OperaErrorCode = faultCode,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = CommonConst.GdsError.OwsFault,
				ErrorValue = faultValue
                    
			},
			Source = faultSource
		};

	/// <summary>
	/// Returning an OwsResult as a consequence of an Http not being 200/OK
	/// </summary>
	/// <param name="httpResponseMessage"></param>
	/// <returns></returns>
	public static OwsResult HttpResult(HttpResponseMessage httpResponseMessage) =>
		new OwsResult()
		{
			ResultStatusFlag = CommonConst.OwsResultStatusFlag.Fail,
			Text = $"Error communicating with OWS: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}",
			OperaErrorCode = CommonConst.OperaErrorCode.Wbs,
			GdsError = new GdsError
			{
				ElementId = CommonConst.GdsError.WbsElementId,
				ErrorCode = CommonConst.GdsError.HttpErrorCode
			}
		};

	/// <summary>
	/// Returns a new LibException based on an OwsError
	/// </summary>
	/// <param name="owsResult"></param>
	/// <param name="callingMethod"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	public static LibException GetLibException(OwsResult owsResult, string callingMethod, string message)
	{
		return new LibException(message, callingMethod)
		{
			OperaErrorCode = owsResult.OperaErrorCode,
			ErrorCodePrefix = owsResult.GdsError?.ElementId ?? string.Empty,
			ErrorCodeSuffix = owsResult.GdsError?.ErrorCode ?? string.Empty,
			ErrorType = owsResult.GdsError?.ErrorType ?? string.Empty,
			ErrorValue = owsResult.GdsError?.ErrorValue ?? string.Empty
		};
	}

	public static LibException GetLibException(Exception ex, string callingMethod, string errorCode, string operaErrorCode)
	{
		var owsResult = GetWbsResult(errorCode, operaErrorCode);
		owsResult.OperaErrorCode = operaErrorCode;
		owsResult.GdsError.ErrorCode = errorCode;

		return GetLibException(owsResult, callingMethod, ex.Message);
	}
}