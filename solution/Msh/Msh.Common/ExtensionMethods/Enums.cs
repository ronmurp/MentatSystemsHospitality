public static class Enums
{
	/// <summary>
	/// Get an IEnumerable of enum values
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public static IEnumerable<T> Get<T>() =>
		System.Enum.GetValues(typeof(T)).Cast<T>();

	/// <summary>
	/// Get a particular enum value corresponding to a string input
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public static T Get<T>(this string value) =>
		Enum.GetValues(typeof(T))
			.Cast<T>()
			.SingleOrDefault(e => e.ToString().ToLower() == value.ToLower());
}