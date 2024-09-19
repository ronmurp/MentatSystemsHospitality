namespace Msh.Common.Services;

/// <summary>
/// Provide basic config data to other services.
/// </summary>
public interface IConfigService
{
	string AppDataPath { get; }
	string MainLogFolder { get; }
	
	string GetEnvironmentVariable(string name, EnvironmentVariableTarget target, string defaultValue);

	/// <summary>
	/// Verifies a test token is available to allow the call to be made
	/// </summary>
	/// <param name="path"></param>
	/// <param name="token"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	bool VerifyTestToken(string id, string path, string token);
}