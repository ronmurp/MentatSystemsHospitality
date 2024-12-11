using Microsoft.Extensions.DependencyInjection;
using Msh.Common.Data;
using Msh.Common.Services;

namespace Msh.Common.Startup;

public static class DiRegistrationCom
{
    public static void RegisterCommonServices(this IServiceCollection services)
    {
        services.AddScoped<IConfigRepository, ConfigRepository>();
        services.AddScoped<ConfigDbContext, ConfigDbContext>();
        services.AddScoped<ICriticalErrorService, CriticalErrorService>();
        services.AddScoped<IConfigStateRepo, ConfigStateRepo>();
        services.AddScoped<ICaptchaConfigRepoService, CaptchaConfigRepoService>();
	}
}