using Microsoft.Extensions.DependencyInjection;
using Msh.Loggers.XmlLogger;

namespace Msh.Loggers.Startup;

public static class DiRegisterLog
{
	public static void RegisterCcServices(this IServiceCollection services)
	{
		services.AddScoped<ILogXmlRepoService, LogXmlRepoService>();
		// services.AddScoped<ILogXmlService, LogXmlService>();
	}
}