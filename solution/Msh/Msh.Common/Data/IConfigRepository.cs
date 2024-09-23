using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public interface IConfigRepository
{
    /// <summary>
    /// Get a config record by ConfigType key
    /// </summary>
    /// <param name="configType"></param>
    /// <returns>Null if not found</returns>
    Task<Config?> GetConfigAsync(string configType);

    Task<ConfigPub?> GetConfigPubAsync(string configType);

	Config? GetConfig(string configType);

	Task<T> GetConfigContentAsync<T>(string configType);
	Task<T> GetConfigPubContentAsync<T>(string configType);

	T GetConfigContent<T>(string configType);

	Task<T> GetConfigContentAsync<T>(string configType, string key);
	T GetConfigContent<T>(string configType, string key);

	/// <summary>
	/// Used by Admin to save the configuration, including Notes
	/// </summary>
	Task SaveConfigAsync(Config config);
	void SaveConfig(Config config);

	/// <summary>
	/// Save the current type = must exist. NullConfigException = thrown if not.
	/// </summary>
	Task SaveConfigAsync<T>(string configType, T value);
	void SaveConfig<T>(string configType, T value);

	void SaveConfig<T>(string configType, string key, T value);
	Task SaveConfigAsync<T>(string configType, string key, T value);


	Task SaveMissingConfigAsync<T>(string configType, T defaultObject);
	void SaveMissingConfig<T>(string configType, T defaultObject);
	

	Task SaveMissingConfigAsync<T>(string configType, string key, T defaultObject);
	void SaveMissingConfig<T>(string configType, string key, T defaultObject);

	/// <summary>
	/// Used by Admin to create a new config
	/// </summary>
	/// <param name="config"></param>
	Task AddConfigAsync(Config config);
	void AddConfig(Config config);

	/// <summary>
	/// Should be used only in admin and tests
	/// </summary>
	Task RemoveConfigAsync(string configType);
	void RemoveConfig(string configType);

	Task<bool> PublishConfig(string configType, string userId);
}

