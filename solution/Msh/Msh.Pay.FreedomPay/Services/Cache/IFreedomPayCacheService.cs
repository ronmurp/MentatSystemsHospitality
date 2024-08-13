using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services.Cache;



/// <summary>
/// Memory cache of FreedomPay configuration
/// </summary>
public partial interface IFreedomPayCacheService
{
    Task<FpConfig> GetFpConfig();

    void ReloadConfig();
}