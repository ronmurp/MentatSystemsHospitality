namespace Msh.Opera.Ows.Models;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Examples:
/// <Result resultStatusFlag="FAIL">
///   <hc:GDSError errorCode="SYS" elementId="81">INVALIDGDS</hc:GDSError>
/// </Result>
///
/// <Result resultStatusFlag="FAIL">
///   <c:OperaErrorCode>PROPERTY_RESTRICTED</c:OperaErrorCode>
///   <hc:GDSError errorCode="25" elementId="PID"></hc:GDSError>
/// </Result>
/// </remarks>
public class GdsError
{
	/// <summary>
	/// From elementId => prefix
	/// </summary>
	public string ElementId { get; set; } = string.Empty;

	/// <summary>
	/// From errorCode => suffix
	/// </summary>
	public string ErrorCode { get; set; } = string.Empty;

	/// <summary>
	/// From element content => ?
	/// </summary>
	public string ErrorValue { get; set; } = string.Empty;

	/// <summary>
	/// From OwsErrors.xml ... and documentation
	/// </summary>
	public string ErrorType { get; set; } = string.Empty;
        
}