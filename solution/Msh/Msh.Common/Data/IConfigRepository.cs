using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public partial interface IConfigRepository
{
    /// <summary>
    /// Get a config record by ConfigType key
    /// </summary>
    /// <param name="configType"></param>
    /// <returns>Null if not found</returns>
    Task<Config?> GetConfigAsync(string configType);

	Task<T> GetConfigContentAsync<T>(string configType);
	
	Task<T> GetConfigContentAsync<T>(string configType, string key);

	Task SaveConfigAsync(Config config);

	/// <summary>
	/// Save the current type = must exist. NullConfigException = thrown if not.
	/// </summary>
	Task SaveConfigAsync<T>(string configType, T value);
	
	Task SaveConfigAsync<T>(string configType, string key, T value);


	Task SaveMissingConfigAsync<T>(string configType, T defaultObject);
	

	Task SaveMissingConfigAsync<T>(string configType, string key, T defaultObject);

	/// <summary>
	/// Used by Admin to create a new config
	/// </summary>
	/// <param name="config"></param>
	Task AddConfigAsync(Config config);
	

	/// <summary>
	/// Should be used only in admin and tests
	/// </summary>
	Task RemoveConfigAsync(string configType);
	
}

