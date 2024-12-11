using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

public abstract class AbstractRepository(IConfigRepository configRepository)
{
	/// <summary>
	/// Required for derived types
	/// </summary>
	protected readonly IConfigRepository ConfigRepository = configRepository;

	/// <summary>
	/// Override to return the ConfigType appropriate for the derived type
	/// </summary>
	/// <returns></returns>
	public abstract string ConfigType();

	protected string HotelConfigType(string configType, string hotelCode) => $"{configType}-{hotelCode}";

	public async Task<List<ConfigArchiveBase>?> ArchivedList() =>
		await ConfigRepository.GetConfigArchiveListAsync(ConfigType());


	public async Task<bool> Publish(string userId, string notes = "") =>
		await ConfigRepository.PublishConfigAsync(ConfigType(), userId, notes);

	public async Task<bool> Archive(string archiveCode, string userId, string notes = "") =>
		await ConfigRepository.ArchiveConfigAsync(ConfigType(), archiveCode, userId, notes);

	public async Task<bool> LockPublished(bool performLock, string userId) =>
		await ConfigRepository.LockPublishConfigAsync(ConfigType(), performLock, userId);

	public async Task<bool> LockArchived(string archiveCode, bool performLock, string userId) =>
		await ConfigRepository.LockArchiveConfigAsync(ConfigType(), archiveCode, performLock, userId);
}