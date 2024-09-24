using Msh.Common.Data;
using Msh.Common.Models.Configuration;

namespace Msh.HotelCache.Services;


/// <summary>
/// A base repo that uses hotelCode to create repo records with a hotelCode extension for the ConfigType property.
/// Derived type need only implement the missing type T specific methods.
/// 
/// </summary>
/// <param name="configRepository"></param>
public abstract class AbstractHotelRepository(IConfigRepository configRepository)
{
	/// <summary>
	/// Required for derived types
	/// </summary>
	protected readonly IConfigRepository ConfigRepository = configRepository;

	/// <summary>
	/// Override to return the ConfigType appropriate for the derived type
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	public abstract string ConfigType(string hotelCode);

	protected string HotelConfigType(string configType, string hotelCode) => $"{configType}-{hotelCode}";

	public async Task<List<ConfigArchiveBase>?> ArchivedList(string hotelCode) =>
		await ConfigRepository.GetConfigArchiveListAsync(ConfigType(hotelCode));


	public async Task<bool> Publish(string hotelCode, string userId) =>
		await ConfigRepository.PublishConfigAsync(ConfigType(hotelCode), userId);

	public async Task<bool> Archive(string hotelCode, string archiveCode, string userId) =>
		await ConfigRepository.ArchiveConfigAsync(ConfigType(hotelCode), archiveCode, userId);

	public async Task<bool> LockPublished(string hotelCode, bool performLock, string userId) =>
		await ConfigRepository.LockPublishConfigAsync(ConfigType(hotelCode), performLock, userId);

	public async Task<bool> LockArchived(string hotelCode, string archiveCode, bool performLock, string userId) =>
		await ConfigRepository.LockArchiveConfigAsync(ConfigType(hotelCode), archiveCode, performLock, userId);


}