namespace Msh.WebApp.Services;

/// <summary>
/// A convenient user service that gets various identity values,
/// depending on whether the user is logged in or not.
/// </summary>
public interface IUserService
{
	string? GetUserId();

	bool IsAuthenticated();

	object GetUsers();
}