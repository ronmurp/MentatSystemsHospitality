namespace Msh.WebApp.Models.Admin;

/// <summary>
/// Parameters to pass to _AdminSaveSuccess
/// </summary>
public class SuccessAlert
{

	public string ModelName { get; set; } = string.Empty;
	public string ReturnPath { get; set; } = string.Empty;

	public string ReturnPathName { get; set; } = string.Empty;

}