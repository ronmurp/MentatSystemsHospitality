using System.Security.Claims;

namespace Msh.WebApp.Services;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
{
	public string? GetUserId() => httpContextAccessor?
		.HttpContext?.User?
		.FindFirstValue(ClaimTypes.Email);

	public bool IsAuthenticated() => httpContextAccessor?
		.HttpContext?.User?
		.Identity?.IsAuthenticated ?? false;
}