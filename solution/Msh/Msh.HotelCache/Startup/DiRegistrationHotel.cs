using Microsoft.Extensions.DependencyInjection;
using Msh.HotelCache.Services;
using Msh.HotelCache.Services.Cache;

namespace Msh.HotelCache.Startup;

public static class DiRegistrationHotel
{
    public static void RegisterHotelServices(this IServiceCollection services)
    {
	   
	    services.AddScoped<IDiscountRepository, DiscountRepository>();
	    services.AddScoped<IExtraRepository, ExtraRepository>();
	    services.AddScoped<IHotelRepository, HotelRepository>();
	    services.AddScoped<IRatePlanRepository, RatePlanRepository>();
	    services.AddScoped<IRatePlanTextRepository, RatePlanTextRepository>();
		services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
	    services.AddScoped<ISpecialsRepository, SpecialsRepository>();
	    services.AddScoped<ITestModelRepository, TestModelRepository>();

		services.AddScoped<IHotelCacheService, HotelCacheService>();
       
	}
}