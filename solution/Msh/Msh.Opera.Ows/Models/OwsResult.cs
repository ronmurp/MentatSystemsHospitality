using Msh.Common.Constants;

namespace Msh.Opera.Ows.Models;

public class OwsResult
{
	public OwsResult()
	{
	}

	public OwsResult(bool isSuccess)
	{
		ResultStatusFlag =
			isSuccess ? CommonConst.OwsResultStatusFlag.Success : CommonConst.OwsResultStatusFlag.Fail;
	}
	/// <summary>
	/// resultStatusFlag: FAIL, SUCCESS
	/// </summary>
	public string ResultStatusFlag { get; set; } = string.Empty;

	public bool IsSuccess => ResultStatusFlag.ToUpper() == CommonConst.OwsResultStatusFlag.Success;
	public string OperaErrorCode { get; set; } = string.Empty;

	public GdsError GdsError { get; set; } = new GdsError();
	public string Text { get; set; } = string.Empty;
	public string Source { get; set; } = string.Empty;

	public string Summary() =>
		GdsError == null
			? $"{Text} {OperaErrorCode}"
			: $"{Text} {OperaErrorCode} {GdsError.ErrorCode} {GdsError.ErrorType} {GdsError.ErrorValue}";
}