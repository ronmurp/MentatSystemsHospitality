using Msh.Common.Models;

namespace Msh.WebApp.Repositories
{
	public class SessionSessionStateRepository(IHttpContextAccessor httpContextAccessor) : ISessionStateRepository
	{
		public string GetValue(string key)
		{
			return httpContextAccessor.HttpContext?.Session?.GetString(key)
			       ?? string.Empty;
		}

		public void SetValue(string key, string value)
		{
			httpContextAccessor.HttpContext?.Session?.SetString(key, value);
		}

		public void Remove(string key)
		{
			httpContextAccessor.HttpContext?.Session?.Remove(key);
		}
	}

}
