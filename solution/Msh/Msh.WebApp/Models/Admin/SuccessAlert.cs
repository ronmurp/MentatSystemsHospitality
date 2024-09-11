namespace Msh.WebApp.Models.Admin;

/// <summary>
/// Parameters to pass to _AdminSaveSuccess
/// </summary>
public class SuccessAlert
{
	/// <summary>
	/// The name of the model
	/// </summary>
	public string ModelName { get; set; } = string.Empty;

	/// <summary>
	/// The return path on success
	/// </summary>
	public string ReturnPath { get; set; } = string.Empty;

	/// <summary>
	/// Return path name is displayed as the anchor content
	/// </summary>
	public string ReturnPathName { get; set; } = string.Empty;


	public string KeyPropertyLabel { get; set; } = string.Empty;
	public string KeyPropertyName { get; set; } = string.Empty;

	public string KeyPropertyValue { get; set; } = string.Empty;
	public string FormAction { get; set; } = String.Empty;
}