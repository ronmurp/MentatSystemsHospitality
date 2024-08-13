using Msh.Common.Data;
using Msh.WebApp.Models;

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

}