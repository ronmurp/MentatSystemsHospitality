using Msh.Common.Data;
using Msh.Common.Models.Configuration;
using Msh.WebApp.Models;
using System.Text.Json;

namespace Msh.TestSupport;

public static class TestConfigUtilities
{
    /// <summary>
    /// Get and instance of the repo
    /// </summary>
    /// <returns></returns>
    public static ConfigRepository GetRepository() =>
        new(new ConfigDbContext(GetConnectionString()));


    /// <summary>
    /// Get the database connection string
    /// </summary>
    /// <returns></returns>
    public static string GetConnectionString() =>
        ApplicationConfiguration.GetSetting("ConnectionStrings:DefaultConnection");

    public static void SaveConfig<T>(string configType, T obj)
    {
        var now = DateTime.Now;

        var json = JsonSerializer.Serialize(obj);

        var sut = TestConfigUtilities.GetRepository();

        var config = sut.GetConfig(configType);

        if (config != null)
        {
            config.Content = json;
            config.Notes = $"Update Import: {now:yyyy-MM-dd HH:mm:ss}";

            sut.SaveConfig(config);
            return;
        }

        sut.AddConfig(new Config { ConfigType = configType, Content = json, Notes = $"Initial Import: {now:yyyy-MM-dd HH:mm:ss}" });

    }

}