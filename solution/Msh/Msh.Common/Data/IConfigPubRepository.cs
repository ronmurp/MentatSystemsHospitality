using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

/// <summary>
/// A Published repository for Config models in ConfigPub
/// This repo, for customer facing ops, can only read the data.
/// IConfigRepository stores data, by 'publishing' edited data
/// </summary>
public interface IConfigPubRepository
{
    /// <summary>
    /// Get a config record by ConfigType key
    /// </summary>
    /// <param name="configType"></param>
    /// <returns>Null if not found</returns>
    Task<ConfigPub?> GetConfigAsync(string configType);

	Task<T> GetConfigContentAsync<T>(string configType);

	Task<T> GetConfigContentAsync<T>(string configType, string key);

}

