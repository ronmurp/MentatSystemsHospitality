using Msh.Pay.FreedomPay.Models.Configuration;
using Msh.Pay.FreedomPay.Models;

namespace Msh.Pay.FreedomPay.Services.Cache;

public partial class FreedomPayCacheService
{
    public async Task<List<PaymentTypeItem>> GetPaymentTypes() => await base.GetData<List<PaymentTypeItem>>(ConstFp.FpPaymentType);

    public void ReloadPaymentTypes() => base.Reload(ConstFp.FpPaymentType);
}
