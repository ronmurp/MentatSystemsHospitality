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
	public async Task<List<ConfigArchiveBase>?> GetConfigArchiveListAsync(string configType)
	{
		var list = configDbContext.ConfigsArchive.Where(a => a.ConfigType.StartsWith($"{configType}="));

			var list2 = await  list
			.Select(a => new ConfigArchiveBase
			{
				ConfigType = a.ConfigType,
				Locked = a.Locked,
				Published = a.Published,
				PublishedBy = a.PublishedBy,
				Notes = a.Notes

			}).ToListAsync();

		list2.ForEach((item) =>
		{
			item.ConfigType = (item.ConfigType.Split('=')[1]);
		});

		return list2;
	}

	public async Task<T> GetConfigArchiveContentAsync<T>(string configType, string archiveCode)
	{
		var archiveType = ArchiveType(configType, archiveCode);

		var config = await GetConfigArchiveAsync(archiveType);
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

	public async Task<bool> ArchiveConfigAsync(string configType, string archiveCode, string userId)
	{
		var archiveType = $"{configType}={archiveCode}";

		var configArchive = await configDbContext.ConfigsArchive.FirstOrDefaultAsync(c => c.ConfigType == archiveType);
		var hasArchive = configArchive != null;

		if (configArchive is { Locked: true })
		{
			return false;
		}
		var config = await configDbContext.Configs.FirstOrDefaultAsync(c => c.ConfigType == configType);

		var configArchiveSave = config.Adapt<ConfigArchive>();
		configArchiveSave.ConfigType = archiveType;
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

	public async Task<bool> LockArchiveConfigAsync(string configType, string archiveCode, bool performLock,
		string userId)
	{
		var archiveType = ArchiveType(configType, archiveCode);

		var config = configDbContext.ConfigsArchive.FirstOrDefault(c => c.ConfigType == archiveType);
		if (config == null)
		{
			throw new NullConfigException($"Remove: Archive Config type not found: {configType} - {archiveCode}");
		}

		config.Locked = performLock;
		configDbContext.ConfigsArchive.Update(config);
		await configDbContext.SaveChangesAsync();

		return true;

	}

	public async Task RemoveConfigArchiveAsync(string configType, string archiveCode)
	{
		var config = configDbContext.ConfigsArchive.FirstOrDefault(c => c.ConfigType == ArchiveType(configType, archiveCode));
		if (config == null)
		{
			throw new NullConfigException($"Remove: Archive Config type not found: {configType}");
		}

		configDbContext.ConfigsArchive.Remove(config);
		await configDbContext.SaveChangesAsync();
	}

	private async Task<ConfigArchive?> GetConfigArchiveAsync(string configType) =>
		await configDbContext.ConfigsArchive.FirstOrDefaultAsync(a => a.ConfigType == configType);

	public string ArchiveType(string configType, string archiveCode) => $"{configType}={archiveCode}";
}