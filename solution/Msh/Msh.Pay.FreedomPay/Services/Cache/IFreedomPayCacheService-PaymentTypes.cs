using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services.Cache;

public partial interface IFreedomPayCacheService
{
    Task<List<PaymentTypeItem>> GetPaymentTypes();

    void ReloadPaymentTypes();
}