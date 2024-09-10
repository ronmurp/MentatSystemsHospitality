using System.Security.Claims;
using Msh.WebApp.Data;

namespace Msh.WebApp.Services;

public class UserService(IHttpContextAccessor httpContextAccessor,
	ApplicationDbContext applicationDbContext) : IUserService
{
	public string? GetUserId() => httpContextAccessor?
		.HttpContext?.User?
		.FindFirstValue(ClaimTypes.Email);

	public bool IsAuthenticated() => httpContextAccessor?
		.HttpContext?.User?
		.Identity?.IsAuthenticated ?? false;

	public object GetUsers()
	{
		var users = applicationDbContext.Users.ToList();

		return users;
	}
}