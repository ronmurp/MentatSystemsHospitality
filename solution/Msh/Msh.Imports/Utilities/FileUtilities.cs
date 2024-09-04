using System.Reflection;

namespace Msh.Imports.Utilities
{
	public static class FileUtilities
	{
		public static string PathBaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

		public static string PathLocation()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		public static string GetProjectPath()
		{
			var d = new DirectoryInfo(PathBaseDirectory);

			return d?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
		}
	}
}
