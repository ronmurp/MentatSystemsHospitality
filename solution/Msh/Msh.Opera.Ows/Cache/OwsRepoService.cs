using Msh.Common.Data;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.OwsErrors;

namespace Msh.Opera.Ows.Cache;

/// <summary>
/// Used in Admin to edit Ows Config and error list
/// </summary>
public class OwsRepoService(IConfigRepository configRepository) : IOwsRepoService
{
	public async Task<OwsConfig> GetOwsConfigAsync() =>
		await configRepository.GetConfigContentAsync<OwsConfig>(OwsConst.Cache.OwsConfig);

	public async Task SaveOwsConfigAsync(OwsConfig owsConfig)
	{
		await configRepository.SaveConfigAsync(OwsConst.Cache.OwsConfig, owsConfig);
	}

	public async Task<OwsErrorList> GetOwsErrorListAsync() =>
		await configRepository.GetConfigContentAsync<OwsErrorList>(OwsConst.Cache.OwsErrorList);

	public async Task SaveOwsErrorListAsync(OwsErrorList owsErrorList)
	{
		await configRepository.SaveConfigAsync(OwsConst.Cache.OwsErrorList, owsErrorList);
	}
}