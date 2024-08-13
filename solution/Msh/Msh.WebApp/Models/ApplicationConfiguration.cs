namespace Msh.WebApp.Models;

/// <summary>
/// Retrieves config settings - used in tests to get connection string
/// </summary>
public static class ApplicationConfiguration
{
    private static IConfigurationRoot _configuration;
    static ApplicationConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        _configuration = builder.Build();
    }
    public static string GetSetting(string key)
    {
        return _configuration[key];
    }
}

