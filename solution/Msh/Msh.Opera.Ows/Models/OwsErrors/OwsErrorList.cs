namespace Msh.Opera.Ows.Models.OwsErrors;

/// <summary>
/// A dictionary of OWS and WBS error messages based on codes returned by OWS, or an WBS substitute codes.
/// </summary>
/// <remarks>
/// They are all related to OWS calls, but when there is some other WBS error, or an unexpected OWS error,
/// WBS error codes may be substituted in order to return friendly error messages to the user.
///
/// All messages depend on a LibException being thrown, or a LibException being constructed to replace another exception. 
/// </remarks>
public class OwsErrorList : Dictionary<string, OwsError>
{
	/// <summary>
	/// The message returned if all other attempts fail or return an empty message.
	/// Can be used to simplify the XML when a number of OWS/WBS errors
	/// </summary>
	public string DefaultErrorMessage { get; set; } = string.Empty;
}