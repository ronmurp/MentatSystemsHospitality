using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services;

/// <summary>
/// Memory cache of FreedomPay configuration
/// </summary>
public interface IFreedomPayCacheService
{
    Task<FpConfig> GetFpConfig();

    Task<List<FpErrorCodeBank>> GetFpErrorCodesBank();

    Task<List<FpErrorCode>> GetFpErrorCodes();

    Task<List<PaymentTypeItem>> GetPaymentTypes();

    Task<List<KmapConfig>> GetFpKmaps();

    void ReloadConfig();

    void ReloadBank();

    void ReloadErrorCodes();

    void ReloadPaymentTypes();

    void ReloadFpKmaps();
}