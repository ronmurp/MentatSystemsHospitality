namespace Msh.Opera.Ows.Models.OwsErrors;

/// <summary>
/// An OWS Error Code is one returned by OWS - or a custom code set by WBS to emulate the error message process
/// so that WBS errors can be included in response to OWS errors
/// </summary>
public class OwsError
{
	public int IntId { get; set; }

	public string Id { get; set; } = string.Empty;

	public string Prefix { get; set; } = string.Empty;

	public string Suffix { get; set; } = string.Empty;

	public string Value { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;

	public string ErrorType { get; set; } = string.Empty;

	public string Key => $"{Prefix}-{Suffix}-{Value}";

	public List<OwsErrorItem> Items { get; set; } = new List<OwsErrorItem>();

	public OwsErrorItem GetErrorItem(string operaErrorCode) => Items?.FirstOrDefault(c => c.ErrorCode == operaErrorCode);

	/// <summary>
	/// If there's an opera error code in Items, return its options
	/// </summary>
	/// <param name="operaErrorCode"></param>
	/// <returns></returns>
	public List<OwsErrorItemOption> GetErrorOptions(string operaErrorCode) => Items?.FirstOrDefault(c => c.ErrorCode == operaErrorCode)?.Options;
}