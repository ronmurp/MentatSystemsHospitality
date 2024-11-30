using Msh.Common.Models.Configuration;

namespace Msh.HotelCache.Services;

public interface IBaseConfigRepository<T>
{
	//------ GET ----------------
	Task<T> GetData();

	Task<T> Published();

	Task<List<ConfigArchiveBase>?> ArchivedList();

	Task<T> Archived(string archiveCode);




	//------ SAVE ---------------------
	Task<bool> Save(T hotels, string notes = "");

	Task<bool> Publish(string userId, string notes = "");

	Task<bool> Archive(string archiveCode, string userId, string notes = "");


	//---- LOCK
	Task<bool> LockPublished(bool performLock, string userId);

	Task<bool> LockArchived(string archiveCode, bool performLock, string userId);

	Task<bool> ArchiveDelete(string archiveCode, string userId);
}