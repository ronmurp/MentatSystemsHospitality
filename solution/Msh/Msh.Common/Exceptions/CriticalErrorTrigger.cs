using System.Net;
using System.Text.Json.Serialization;

namespace Msh.Common.Exceptions;

/// <summary>
/// Identifies critical errors in API returned contents that should be checked before deserialization.
/// </summary>
public class CriticalErrorTrigger
{
	/// <summary>
	/// The error code under which to log the error
	/// </summary>
	public string Code { get; set; } = "UNKNOWN-CODE";

	/// <summary>
	/// The trigger text. Defaults to a Contents.Contains(Trigger) test, but uses regex if IsRegex is true
	/// </summary>
	public string Trigger { get; set; } = string.Empty;

	public bool IsRegEx { get; set; } = false;


	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CriticalErrorType ErrorType { get; set; } = CriticalErrorType.Critical;


	// [JsonConverter(typeof(EnumListConverter<CriticalErrorCall>))]
	public List<CriticalErrorCall> CallTypes { get; set; } = [];

	public string UserMessage { get; set; } = string.Empty;

	public string ValueType { get; set; } = string.Empty;


	[JsonConverter(typeof(JsonStringEnumConverter))]
	public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.NotFound;

	/// <summary>
	/// Any sub type present may override the user message.
	/// If the user message here is empty, the main user message is used.
	/// Sub type key can be derived from API supplied errors
	/// </summary>
	public List<CriticalErrorSubType> SubTypes { get; set; } = new List<CriticalErrorSubType>();

	/// <summary>
	/// Notes for ELH Admin users on what the error means => what the user error should be
	/// </summary>
	public string Notes { get; set; } = string.Empty;

	
}