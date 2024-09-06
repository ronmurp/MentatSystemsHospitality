namespace Msh.Common.Models.ViewModels;

public class BaseVm
{
	public BaseVm()
	{

	}

	public BaseVm(string errorMessage)
	{
		Success = false;
		ErrorMessage = errorMessage;
	}
	public bool Success { get; set; } = true;
	public string ErrorMessage { get; set; } = string.Empty;

	/// <summary>
	/// A message (may be html) that can be returned on success.
	/// </summary>
	public string Message { get; set; } = string.Empty;
	/// <summary>
	/// A message (may be html) that can be returned on success.
	/// </summary>
	public string UserErrorMessage { get; set; } = string.Empty;
	public bool UserMessageIsHtml { get; set; }
	/// <summary>
	/// Any data that might need to be added
	/// </summary>
	public object? Data { get; set; }
}