using Microsoft.Extensions.DependencyInjection;
using Msh.Pay.CoinCorner.Services;

namespace Msh.Pay.CoinCorner.Startup;

public static class DiRegistrationCc
{
    public static void RegisterCcServices(this IServiceCollection services)
    {
        services.AddScoped<ICoinCornerCacheService, CoinCornerCacheService>();
    }
}