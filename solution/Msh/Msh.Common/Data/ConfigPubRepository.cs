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
	/// Get a config record by ConfigType and key
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="configType"></param>
	/// <param name="key">An extra key, where config is broken down by key - e.g. room types for each hotel</param>
	/// <returns></returns>
	public async Task<T> GetConfigContentAsync<T>(string configType, string key) => 
		await GetConfigContentAsync<T>($"{configType}-{key}");

}