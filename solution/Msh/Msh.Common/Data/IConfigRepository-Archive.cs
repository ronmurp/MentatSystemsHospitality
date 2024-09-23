using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

public partial interface IConfigRepository
{
	Task<ConfigArchive?> GetConfigArchiveAsync(string configType);

	Task<T> GetConfigArchiveContentAsync<T>(string configType);

	/// <summary>
	/// Copy data from the editing Config table to the ConfigPub table
	/// </summary>
	/// <param name="configType"></param>
	/// <param name="userId"></param>
	/// <returns></returns>
	Task<bool> SaveConfigArchiveAsync(string configType, string userId);

	Task RemoveConfigArchiveAsync(string configType);
}