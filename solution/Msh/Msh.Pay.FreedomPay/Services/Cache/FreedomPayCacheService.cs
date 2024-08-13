using Microsoft.Extensions.Caching.Memory;
using Msh.Common.Data;
using Msh.Common.Services;
using Msh.Pay.FreedomPay.Models;
using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services.Cache;

/// <summary>
/// Memory cache of FreedomPay configuration
/// </summary>
/// <param name="memoryCache"></param>
/// <param name="configRepository"></param>
public partial class FreedomPayCacheService(IMemoryCache memoryCache, IConfigRepository configRepository)
    : DataCacheService(memoryCache, configRepository), IFreedomPayCacheService
{

    public async Task<FpConfig> GetFpConfig() => 
        await base.GetData<FpConfig>(ConstFp.FpConfig);
   
    public void ReloadConfig() => base.Reload(ConstFp.FpConfig);
}