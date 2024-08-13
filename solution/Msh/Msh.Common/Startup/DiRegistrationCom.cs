using Microsoft.Extensions.DependencyInjection;
using Msh.Common.Data;

namespace Msh.Common.Startup;

public static class DiRegistrationCom
{
    public static void RegisterCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IConfigRepository, ConfigRepository>();
        services.AddScoped<ConfigDbContext, ConfigDbContext>();
    }
}