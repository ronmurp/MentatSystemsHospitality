using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.OwsErrors;

namespace Msh.Opera.Ows.Cache;

/// <summary>
/// Return (or reload) Hotel
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public class OwsCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
	: DataCacheService(memoryCache, configRepository), IOwsCacheService
{
	
	public async Task<OwsConfig> GetOwsConfig() =>
		await base.GetData<OwsConfig>(OwsConst.Cache.OwsConfig);
	public void ReloadOwsConfig() => base.Reload(OwsConst.Cache.OwsConfig);

	public async Task<OwsErrorList> GetOwsErrorList() =>
		await base.GetData<OwsErrorList>(OwsConst.Cache.OwsConfig);
	public void ReloadOwsErrorList() => base.Reload(OwsConst.Cache.OwsErrorList);
}