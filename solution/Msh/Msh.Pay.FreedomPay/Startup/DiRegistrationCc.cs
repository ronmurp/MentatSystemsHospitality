using Microsoft.Extensions.DependencyInjection;
using Msh.Pay.FreedomPay.Services;
using Msh.Pay.FreedomPay.Services.Cache;

namespace Msh.Pay.FreedomPay.Startup;

public static class DiRegistrationCc
{
    public static void RegisterFpServices(this IServiceCollection services)
    {
        services.AddScoped<IFreedomPayCacheService, FreedomPayCacheService>();
        services.AddScoped<IFpRepoService, FpRepoService>();
    }
}