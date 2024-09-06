namespace Msh.WebApp.Models.Admin;

/// <summary>
/// How an error message is passed to an error view
/// </summary>
public class ErrorBlock
{
	public string Message { get; set; } = string.Empty;
	public string CssClass { get; set; } = "error-block";
}