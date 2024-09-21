namespace Msh.Opera.Ows.Models.OwsErrors;


/// <summary>
/// A prefix-suffix combination might have a number of error codes.
/// Use them if found, and if there's any message, otherwise use the default
/// in the parent
/// </summary>
public class OwsErrorItem
{
	public string ErrorCode { get; set; }
	public List<OwsErrorItemOption> Options { get; set; }

	/// <summary>
	/// Assume it's an error, unless this flag is false: code value="PROPERTY_NOT_AVAILABLE" error="false" in OwsErrorCodes.xml
	/// </summary>
	public bool IsError { get; set; } = true;

	public int Retries { get; set; } = 0;
}