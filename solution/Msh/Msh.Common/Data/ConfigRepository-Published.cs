using Mapster;
using Microsoft.EntityFrameworkCore;
using Msh.Common.Exceptions;
using Msh.Common.Models.Configuration;
using System.Text.Json;

namespace Msh.Common.Data;

/// <summary>
/// Part of the editing ConfigRepository that in Admin interacts with the ConfigPub data
/// </summary>
public partial class ConfigRepository
{
	public async Task<ConfigPub?> GetConfigPubAsync(string configType) =>
		await configDbContext.ConfigsPub.FirstOrDefaultAsync(a => a.ConfigType == configType);

	public async Task<T> GetConfigPubContentAsync<T>(string configType)
	{
		var config = await GetConfigPubAsync(configType);
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

	public async Task<bool> PublishConfigAsync(string configType, string userId, string notes = "")
	{
		var configPub = await configDbContext.ConfigsPub.FirstOrDefaultAsync(c => c.ConfigType == configType);
		var hasPub = configPub != null;

		if (configPub is { Locked: true })
		{
			return false;
		}
		var config = await configDbContext.Configs.FirstOrDefaultAsync(c => c.ConfigType == configType);

		var configPubSave = config.Adapt<ConfigPub>();
		configPubSave.Locked = true;
		configPubSave.Published = DateTime.Now;
		configPubSave.PublishedBy = userId;
		configPubSave.Notes = notes;
		if (hasPub)
			configDbContext.ConfigsPub.Update(configPubSave);
		else
			await configDbContext.ConfigsPub.AddAsync(configPubSave);
		await configDbContext.SaveChangesAsync();

		return true;
	}

	public async Task SavePublishConfigAsync(ConfigPub configPub)
	{
		configDbContext.ConfigsPub.Update(configPub);
		await configDbContext.SaveChangesAsync();
	}

	public async Task RemoveConfigPubAsync(string configType)
	{
		var config = configDbContext.ConfigsPub.FirstOrDefault(c => c.ConfigType == configType);
		if (config == null)
		{
			throw new NullConfigException($"Remove: Published Config type not found: {configType}");
		}

		configDbContext.ConfigsPub.Remove(config);
		await configDbContext.SaveChangesAsync();
	}

	public async Task<bool> LockPublishConfigAsync(string configType, bool performLock, string userId)
	{
		var config = configDbContext.ConfigsPub.FirstOrDefault(c => c.ConfigType == configType);
		if (config == null)
		{
			throw new NullConfigException($"Remove: Published Config type not found: {configType}");
		}

		config.Locked = performLock;
		configDbContext.ConfigsPub.Update(config);
		await configDbContext.SaveChangesAsync();

		return true;
	}
}