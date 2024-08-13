using Msh.Pay.FreedomPay.Models.Configuration;
using Msh.Pay.FreedomPay.Models;

namespace Msh.Pay.FreedomPay.Services.Cache;

public partial class FreedomPayCacheService
{
    public async Task<List<KmapConfig>> GetFpKmaps() =>
        await base.GetData<List<KmapConfig>>(ConstFp.FpKmapConfig);

    public void ReloadFpKmaps() => base.Reload(ConstFp.FpKmapConfig);
}
