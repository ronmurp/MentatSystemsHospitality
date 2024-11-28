using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

public partial interface IConfigRepository
{
	Task<ConfigPub?> GetConfigPubAsync(string configType);

	Task<T> GetConfigPubContentAsync<T>(string configType);

	/// <summary>
	/// Copy data from the editing Config table to the ConfigPub table
	/// </summary>
	/// <param name="configType"></param>
	/// <param name="userId"></param>
	/// <param name="notes"></param>
	/// <returns></returns>
	Task<bool> PublishConfigAsync(string configType, string userId, string notes = "");

	Task SavePublishConfigAsync(ConfigPub configPub);


	Task RemoveConfigPubAsync(string configType);

	Task<bool> LockPublishConfigAsync(string configType, bool performLock, string userId);
}