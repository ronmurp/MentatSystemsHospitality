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
    Config? GetConfig(string configType);

    T GetConfigContent<T>(string configType);

    /// <summary>
    /// Used by Admin to save the configuration, including Notes
    /// </summary>
    void SaveConfig(Config config);

    /// <summary>
    /// Save the current type = must exist. NullConfigException = thrown if not.
    /// </summary>
    void SaveConfig<T>(string configType, T value);


    void SaveMissingConfig<T>(string configType, T defaultObject);

	/// <summary>
	/// Used by Admin to create a new config
	/// </summary>
	/// <param name="config"></param>
	void AddConfig(Config config);

    /// <summary>
    /// Should be used only in admin and tests
    /// </summary>
    void RemoveConfig(string configType);
}