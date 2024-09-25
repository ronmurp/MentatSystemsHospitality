using Msh.Common.Models.Configuration;

namespace Msh.HotelCache.Services;

/// <summary>
/// Interface for config repositories that have a hotelCode appended to the ConfigType
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseHotelRepository<T>
{
	string ConfigType(string hotelCode);

	//--- 1 x 4 ------------
	/// <summary>
	/// Loads the complete record - from which a select may be build.
	/// </summary>
	/// <param name="hotelCode"></param>
	/// <returns></returns>
	Task<List<ConfigArchiveBase>?> ArchivedList(string hotelCode);
	Task<List<T>> Archived(string hotelCode, string archiveCode);
	Task<List<T>> Published(string hotelCode);
	Task<List<T>> GetData(string hotelCode);

	//- 2 Saves
	Task<bool> Save(List<T> items, string hotelCode);
	Task<bool> Publish(string hotelCode, string userId);
	Task<bool> Archive(string hotelCode, string archiveCode, string userId);

	//- 3 - Lock
	Task<bool> LockPublished(string hotelCode, bool performLock, string userId);
	Task<bool> LockArchived(string hotelCode, string archiveCode, bool performLock, string userId);
}


