using Msh.Common.Startup;

namespace Msh.WebApp.Startup;

public static class DiConfiguration
{
    public static void LoadServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.RegisterCommonServices();
    }
}
