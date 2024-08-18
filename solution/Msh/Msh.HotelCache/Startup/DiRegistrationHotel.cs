using Microsoft.Extensions.DependencyInjection;
using Msh.HotelCache.Services;
using Msh.HotelCache.Services.Cache;

namespace Msh.HotelCache.Startup;

public static class DiRegistrationHotel
{
    public static void RegisterHotelServices(this IServiceCollection services)
    {
        services.AddScoped<IHotelCacheService, HotelCacheService>();
        services.AddScoped<IHotelsRepoService, HotelsRepoService>();
	}
}