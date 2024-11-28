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

	Task<bool> SaveConfigAsync(Config config);

	/// <summary>
	/// Save the current type = must exist. NullConfigException = thrown if not.
	/// </summary>
	Task<bool> SaveConfigAsync<T>(string configType, T value, string notes = "");

	Task<bool> DeleteConfigAsync(string configType);

	Task<bool> SaveMissingConfigAsync<T>(string configType, T defaultObject);
	

	/// <summary>
	/// Used by Admin to create a new config
	/// </summary>
	/// <param name="config"></param>
	Task<bool> AddConfigAsync(Config config);
	

	/// <summary>
	/// Should be used only in admin and tests
	/// </summary>
	Task<bool> RemoveConfigAsync(string configType);

	/// <summary>
	/// return a constructed configType based on configType + hotelCode
	/// </summary>
	/// <param name="configType"></param>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	string HotelType(string configType, string hotelCode);

	string ArchiveType(string configType, string archiveCode);

}

