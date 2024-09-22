using Msh.Common.Constants;
using Msh.Common.Data;
using Msh.Common.Models.Configuration;

namespace Msh.Common.Services;

/// <summary>
/// A place to record various states that other records can be recorded in.
/// </summary>
public class ConfigStateRepo(IConfigRepository configRepository) : IConfigStateRepo
{
	public async Task<List<ConfigState>> GetConfigState() =>
		await configRepository.GetConfigContentAsync<List<ConfigState>>($"{RepoConst.ConfigState}");

	public async Task SaveConfigState(List<ConfigState> list)
	{
		if (list.All(s => s.Code != "Pub"))
		{
			list.Insert(0, new ConfigState { Code = "Pub", Description = "Publish state. Reserved."});
		}
		
		await configRepository.SaveConfigAsync($"{RepoConst.ConfigState}", list);
	}
}