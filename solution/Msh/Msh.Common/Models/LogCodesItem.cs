namespace Msh.Common.Models;

/// <summary>
/// Used in a list of log codes to determine if something should be logged
/// </summary>
/// <remarks>
/// WbsLogger.Info(code, message);
/// If the LogCodesItem.Code has the corresponding LogCodesItem.Enabled true, the message will be logged.
/// </remarks>
public class LogCodesItem
{
	/// <summary>
	/// The value of the field, as text
	/// </summary>
	public string Code { get; set; } = string.Empty;
	public bool Enabled { get; set; }

	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// The name of the LogCodes field
	/// </summary>
	public string Name { get; set; }
}