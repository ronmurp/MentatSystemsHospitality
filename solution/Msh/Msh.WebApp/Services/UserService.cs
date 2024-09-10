using System.Security.Claims;
using Msh.Admin.Models.ViewModels;
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

	public List<UserVm> GetUsers()
	{
		var users = applicationDbContext.Users
			.Select(u => new UserVm
		{
				Email = u.Email ?? string.Empty,
				IsConfirmed = u.EmailConfirmed
		}).ToList();

		return users;
	}
}