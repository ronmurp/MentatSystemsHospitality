using Msh.Common.ExtensionMethods;

namespace Msh.Common.Services;

/// <summary>
/// Provides simple helpful methods
/// </summary>
public static class CommonHelper
{

	/// <summary>
	/// Returns a new GUID formatted as N
	/// </summary>
	/// <returns></returns>
	public static string NewGuidN() => Guid.NewGuid().ToString("N");

	/// <summary>
	/// return a new GUID formatted N, if value empty; or return value
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string NewGuidIfEmpty(this string value) => value.Empty() ? Guid.NewGuid().ToString("N") : value;
}