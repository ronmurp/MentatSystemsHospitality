using Microsoft.Extensions.DependencyInjection;
using Msh.Admin.Services;

namespace Msh.Admin.Startup;

public static class DiRegistrationAdmin
{
    public static void RegisterAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminVmService, AdminVmService>();
    }
}