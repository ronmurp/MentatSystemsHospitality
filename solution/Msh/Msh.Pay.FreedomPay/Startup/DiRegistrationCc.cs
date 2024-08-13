using Microsoft.Extensions.DependencyInjection;
using Msh.Pay.FreedomPay.Services;

namespace Msh.Pay.FreedomPay.Startup;

public static class DiRegistrationCc
{
    public static void RegisterFpServices(this IServiceCollection services)
    {
        services.AddScoped<IFreedomPayCacheService, FreedomPayCacheService>();
    }
}