using Msh.Common.Models.Configuration;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Msh.Common.Exceptions;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public class ConfigPubRepository(ConfigDbContext configDbContext) : IConfigPubRepository
{
	/// <summary>
	/// Get a config record by ConfigType
	/// </summary>
	/// <param name="configType"></param>
	/// <returns>Null if not found</returns>
	public async Task<ConfigPub?> GetConfigAsync(string configType) => 
		await configDbContext.ConfigsPub.FirstOrDefaultAsync(a => a.ConfigType == configType);

	public ConfigPub? GetConfig(string configType) => 
		configDbContext.ConfigsPub.FirstOrDefault(a => a.ConfigType == configType);

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

	public T GetConfigContent<T>(string configType)
	{
		var config = GetConfig(configType);
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
	/// Get a config record by ConfigType and key
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="configType"></param>
	/// <param name="key">An extra key, where config is broken down by key - e.g. room types for each hotel</param>
	/// <returns></returns>
	public async Task<T> GetConfigContentAsync<T>(string configType, string key) => 
		await GetConfigContentAsync<T>($"{configType}-{key}");

	public T GetConfigContent<T>(string configType, string key) => 
		GetConfigContent<T>($"{configType}-{key}");


	/// <summary>
	/// Used by Admin to save the configuration, including Notes
	/// </summary>
	/// <param name="config"></param>
	public async Task SaveConfigAsync(ConfigPub config)
	{
		configDbContext.ConfigsPub.Update(config);
		await configDbContext.SaveChangesAsync();
	}

	public void SaveConfig(ConfigPub config)
	{
		configDbContext.ConfigsPub.Update(config);
		configDbContext.SaveChanges();
	}

	public async Task SaveConfigAsync<T>(string configType, T value)
	{
		var json = JsonSerializer.Serialize(value);
		var config = await GetConfigAsync(configType);
		if (config == null)
		{
			config = new ConfigPub { ConfigType = configType, Content = json };
			await AddConfigAsync(config);
		}
		else
		{
			config.Content = json;
			await SaveConfigAsync(config);
		}

	}

	public void SaveConfig<T>(string configType, T value)
	{
		var json = JsonSerializer.Serialize(value);
		var config = GetConfig(configType);
		if (config == null)
		{
			config = new ConfigPub { ConfigType = configType, Content = json };
			AddConfig(config);
		}
		else
		{
			config.Content = json;
			SaveConfig(config);
		}
	}


	/// <summary>
	/// Save the current type = must exist. NullConfigException = thrown if not.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="configType"></param>
	/// <param name="value"></param>
	/// <exception cref="NullConfigException"></exception>
	public async Task SaveConfigAsync<T>(string configType, string key, T value)
	{
		await SaveConfigAsync<T>($"{configType}-{key}", value);
	}

	public void SaveConfig<T>(string configType, string key, T value)
	{
		SaveConfig<T>($"{configType}-{key}", value);
	}

	public async Task SaveMissingConfigAsync<T>(string configType, T defaultObject)
	{
		var config = await GetConfigAsync(configType);
		if (config == null)
		{
			var json = JsonSerializer.Serialize(defaultObject);
			config = new ConfigPub
			{
				ConfigType = configType,
				Content = json
			};
			await AddConfigAsync(config);
		}
	}

	public void SaveMissingConfig<T>(string configType, T defaultObject)
	{
		var config = GetConfig(configType);
		if (config == null)
		{
			var json = JsonSerializer.Serialize(defaultObject);
			config = new ConfigPub
			{
				ConfigType = configType,
				Content = json
			}; 
			AddConfig(config);
		}
	}

	public async Task SaveMissingConfigAsync<T>(string configType, string key, T defaultObject)
	{
		await SaveMissingConfigAsync<T>($"{configType}-{key}", defaultObject);
	}

    public void SaveMissingConfig<T>(string configType, string key, T defaultObject)
    {
	    SaveMissingConfig<T>($"{configType}-{key}", defaultObject);
    }

    /// <summary>
    /// Used by Admin to create a new config
    /// </summary>
    /// <param name="config"></param>
	public async Task AddConfigAsync(ConfigPub config)
    {
		configDbContext.ConfigsPub.Add(config);
		await configDbContext.SaveChangesAsync();
    }

    public void AddConfig(ConfigPub config)
    {
        configDbContext.ConfigsPub.Add(config);
        configDbContext.SaveChanges();
    }

    public Task RemoveConfigAsync(string configType)
    {
	    throw new NotImplementedException();
    }

    /// <summary>
    /// Should be used only in admin and tests
    /// </summary>
    /// <param name="configType"></param>
    /// <exception cref="NullConfigException"></exception>
    public void RemoveConfig(string configType)
    {
        var config = configDbContext.ConfigsPub.FirstOrDefault(c => c.ConfigType == configType);
        if (config == null)
        {
            throw new NullConfigException($"Remove: Config type not found: {configType}");
        }

        if (config.Locked)
        {
	        throw new Exception($"The published config type {configType} is locked.");
        }
        configDbContext.ConfigsPub.Remove(config);
        configDbContext.SaveChanges();
    }
}