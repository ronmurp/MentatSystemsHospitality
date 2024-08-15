namespace Msh.Common.Models;

/// <summary>
/// A repository for session state
/// </summary>
public interface ISessionStateRepository
{
	public void SetValue(string key, string value);
	public string GetValue(string key);
	public void Remove(string key);
}