using Msh.Common.Models;
using Msh.Common.Startup;
using Msh.HotelCache.Startup;
using Msh.Pay.CoinCorner.Startup;
using Msh.Pay.FreedomPay.Startup;
using Msh.WebApp.Repositories;

namespace Msh.WebApp.Startup;

public static class DiConfiguration
{
    public static void LoadServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

		services.AddScoped<ISessionStateRepository, SessionSessionStateRepository>();

		services.RegisterCommonServices();
        services.RegisterFpServices();
        services.RegisterCcServices();
        services.RegisterHotelServices();
    }
}
