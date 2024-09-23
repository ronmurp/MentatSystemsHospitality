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
	public async Task<ConfigArchive?> GetConfigArchiveAsync(string configType) =>
		await configDbContext.ConfigsArchive.FirstOrDefaultAsync(a => a.ConfigType == configType);

	public async Task<T> GetConfigArchiveContentAsync<T>(string configType)
	{
		var config = await GetConfigArchiveAsync(configType);
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

	public async Task<bool> SaveConfigArchiveAsync(string configType, string userId)
	{
		var configArchive = await configDbContext.ConfigsArchive.FirstOrDefaultAsync(c => c.ConfigType == configType);
		var hasArchive = configArchive != null;

		if (configArchive is { Locked: true })
		{
			return false;
		}
		var config = await configDbContext.Configs.FirstOrDefaultAsync(c => c.ConfigType == configType);

		var configArchiveSave = config.Adapt<ConfigArchive>();
		configArchiveSave.Locked = true;
		configArchiveSave.Published = DateTime.Now;
		configArchiveSave.PublishedBy = userId;
		if (hasArchive)
			configDbContext.ConfigsArchive.Update(configArchiveSave);
		else
			await configDbContext.ConfigsArchive.AddAsync(configArchiveSave);
		await configDbContext.SaveChangesAsync();

		return true;
	}

	public async Task RemoveConfigArchiveAsync(string configType)
	{
		var config = configDbContext.ConfigsArchive.FirstOrDefault(c => c.ConfigType == configType);
		if (config == null)
		{
			throw new NullConfigException($"Remove: Archive Config type not found: {configType}");
		}

		configDbContext.ConfigsArchive.Remove(config);
		await configDbContext.SaveChangesAsync();
	}

}