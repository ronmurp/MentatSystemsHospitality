using Msh.Common.Models.Configuration;

namespace Msh.Common.Data
{
	/// <summary>
	/// Interface for config repositories that have a hotelCode appended to the ConfigType
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IBaseRepository<T>
	{

		Task<List<ConfigArchiveBase>?> ArchivedList();

		//--- 1 x 4 ------------
		/// <summary>
		/// Loads the complete record - from which a select may be build.
		/// </summary>
		/// <returns></returns>
		Task<ConfigArchiveBase?> Archived();
		Task<T> Archived(string archiveCode);
		Task<T> Published();
		Task<T> GetData();

		//- 2 Saves
		Task<bool> Save(T config, string notes = "");
		Task<bool> Publish(string userId, string notes = "");
		Task<bool> Archive(string archiveCode, string userId, string notes = "");

		Task<bool> ArchiveDelete(string archiveCode, string userId);

		//- 3 - Lock
		Task<bool> LockPublished(bool performLock, string userId);
		Task<bool> LockArchived(string archiveCode, bool performLock, string userId);
	}
}
