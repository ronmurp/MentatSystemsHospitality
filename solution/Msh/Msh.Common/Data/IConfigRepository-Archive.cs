using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

public partial interface IConfigRepository
{
	Task<List<ConfigArchiveBase>?> GetConfigArchiveListAsync(string configType);
	
	Task<T> GetConfigArchiveContentAsync<T>(string configType, string archiveCode);

	/// <summary>
	/// Copy data from the editing Config table to the ConfigPub table
	/// </summary>
	/// <param name="configType"></param>
	/// <param name="archiveCode"></param>
	/// <param name="userId"></param>
	/// <param name="notes"></param>
	/// <returns></returns>
	Task<bool> ArchiveConfigAsync(string configType, string archiveCode, string userId, string notes = "");
		
	Task<bool> DeleteArchiveConfigAsync(string configType, string archiveCode, string userId);

	Task<bool> LockArchiveConfigAsync(string configType, string archiveCode, bool performLock, string userId);
}