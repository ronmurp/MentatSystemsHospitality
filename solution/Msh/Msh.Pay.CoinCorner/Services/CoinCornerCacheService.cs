using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Return (or reload) CoinCornerConfig & CoinCornerGlobal
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public class CoinCornerCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
    : DataCacheService(memoryCache, configRepository), ICoinCornerCacheService
{
    public async Task<CoinCornerConfig> GetCcConfig() => await base.GetData<CoinCornerConfig>(ConstCc.CcConfig);

    public async Task<CoinCornerGlobal> GetCcGlobal() => await base.GetData<CoinCornerGlobal>(ConstCc.CcGlobal);

    public void ReloadConfig() => base.Reload(ConstCc.CcConfig);

    public void ReloadGlobal() => base.Reload(ConstCc.CcGlobal);
}