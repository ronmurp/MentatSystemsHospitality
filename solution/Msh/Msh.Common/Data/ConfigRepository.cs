using Msh.Common.Models.Configuration;
using System.Text.Json;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Msh.Common.Exceptions;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public partial class ConfigRepository(ConfigDbContext configDbContext) : IConfigRepository
{
	/// <summary>
	/// Get a config record by ConfigType
	/// </summary>
	/// <param name="configType"></param>
	/// <returns>Null if not found</returns>
	public async Task<Config?> GetConfigAsync(string configType) => 
		await configDbContext.Configs.FirstOrDefaultAsync(a => a.ConfigType == configType);
	
	/// <summary>
	/// Get the config content for a particular type
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="configType"></param>
	/// <returns></returns>
	public async Task<T> GetConfigContentAsync<T>(string configType)
	{
		var config = await GetConfigAsync(configType);
		if (config == null)
		{
			return default!;
		}

		var obj = JsonSerializer.Deserialize<T>(config.Content);
		if (obj == null)
		{
			return default!;
		}

		return obj;
	}


	/// <summary>
	/// Used by Admin to save the configuration, including Notes
	/// </summary>
	/// <param name="config"></param>
	public async Task<bool> SaveConfigAsync(Config config)
	{
		configDbContext.Configs.Update(config);
		await configDbContext.SaveChangesAsync();
		return true;
	}


	public async Task<bool> SaveConfigAsync<T>(string configType, T value)
	{
		var json = JsonSerializer.Serialize(value);
		var config = await GetConfigAsync(configType);
		if (config == null)
		{
			config = new Config { ConfigType = configType, Content = json };
			await AddConfigAsync(config);
		}
		else
		{
			config.Content = json;
			await SaveConfigAsync(config);
		}

		return true;
	}
	
	public async Task<bool> SaveMissingConfigAsync<T>(string configType, T defaultObject)
	{
		var config = await GetConfigAsync(configType);
		if (config == null)
		{
			var json = JsonSerializer.Serialize(defaultObject);
			config = new Config
			{
				ConfigType = configType,
				Content = json
			};
			await AddConfigAsync(config);
		}

		return true;
	}

    /// <summary>
    /// Used by Admin to create a new config
    /// </summary>
    /// <param name="config"></param>
	public async Task<bool> AddConfigAsync(Config config)
    {
		configDbContext.Configs.Add(config);
		await configDbContext.SaveChangesAsync();

		return true;
    }

    
    /// <summary>
    /// Should be used only in admin and tests
    /// </summary>
    /// <param name="configType"></param>
    /// <exception cref="NullConfigException"></exception>
	public async Task<bool> RemoveConfigAsync(string configType)
    {
		var config = configDbContext.Configs.FirstOrDefault(c => c.ConfigType == configType);
		if (config == null)
		{
			throw new NullConfigException($"Remove: Config type not found: {configType}");
		}

		configDbContext.Configs.Remove(config);
		await configDbContext.SaveChangesAsync();

		return true;
    }

	/// <summary>
	/// return a constructed configType based on configType + hotelCode
	/// </summary>
	/// <param name="configType"></param>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	public string HotelType(string configType, string hotelCode) => $"{configType}-{hotelCode}";

}