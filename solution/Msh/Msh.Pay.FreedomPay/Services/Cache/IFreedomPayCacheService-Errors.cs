using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services.Cache;

public partial interface IFreedomPayCacheService
{
    Task<List<FpErrorCodeBank>> GetFpErrorCodesBank();

    Task<List<FpErrorCode>> GetFpErrorCodes();

    void ReloadBank();

    void ReloadErrorCodes();

}
