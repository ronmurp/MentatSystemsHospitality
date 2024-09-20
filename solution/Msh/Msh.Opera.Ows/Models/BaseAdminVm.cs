namespace Msh.Opera.Ows.Models;

/// <summary>
/// Base view model for admin pages that (might) have an error message field, or might test HasToken
/// </summary>
public class BaseAdminVm
{
	public bool Success { get; set; } = true;
	public string ErrorMessage { get; set; } = string.Empty;

	public string HtmlMarkup { get; set; } = string.Empty;
}