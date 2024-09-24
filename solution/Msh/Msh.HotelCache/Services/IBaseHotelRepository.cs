using Msh.Common.Models.Configuration;

namespace Msh.HotelCache.Services;

public interface IBaseHotelRepository<T>
{
	string ConfigType(string hotelCode);

	//--- 1 x 4 ------------
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


