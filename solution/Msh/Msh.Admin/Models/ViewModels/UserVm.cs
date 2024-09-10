namespace Msh.Admin.Models.ViewModels;

/// <summary>
/// A view model for users in Admin
/// </summary>
public class UserVm
{
	public string Email { get; set; } = string.Empty;

	public bool IsConfirmed { get; set; }

}