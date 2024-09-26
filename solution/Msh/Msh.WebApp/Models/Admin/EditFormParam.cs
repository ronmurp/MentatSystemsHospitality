namespace Msh.WebApp.Models.Admin;


/// <summary>
/// Parameters to pass to _AdminSaveSuccess and _EditCode
/// </summary>
public class EditFormParam
{
	/// <summary>
	/// The name of the model
	/// </summary>
	public string ModelName { get; set; } = string.Empty;

	/// <summary>
	/// The return path on success
	/// </summary>
	public string ReturnUrl { get; set; } = string.Empty;

	/// <summary>
	/// Return path name is displayed as the anchor content
	/// </summary>
	public string ReturnText { get; set; } = string.Empty;


	
	public string FormAction { get; set; } = String.Empty;

	public bool IsEdit => FormAction.EndsWith("Edit");
	public string KeyPropertyLabel { get; set; } = string.Empty;
	public string KeyPropertyName { get; set; } = string.Empty;

	public string KeyPropertyValue { get; set; } = string.Empty;
}
	