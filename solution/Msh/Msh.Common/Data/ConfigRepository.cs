using Msh.Common.Models.Configuration;
using System.Text.Json;
using Msh.Common.Exceptions;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public class ConfigRepository : IConfigRepository
{
    private readonly ConfigDbContext _configDbContext;

    public ConfigRepository(ConfigDbContext configDbContext)
    {
        _configDbContext = configDbContext;
    }

    public Config? GetConfig(string configType)
    {
        return _configDbContext.Configs.FirstOrDefault(a => a.ConfigType == configType) ?? new Config();
    }

    public void SaveConfig<T>(string configType, T value)
    {
        var json = JsonSerializer.Serialize(value);
        var config = GetConfig(configType);
        if (config == null)
        {
            throw new NullConfigException($"Config type not found: {configType}");
        }
        config.Content = json;
        SaveConfig(config);
    }

    /// <summary>
    /// Used by Admin to save the configuration, including Notes
    /// </summary>
    /// <param name="config"></param>
    public void SaveConfig(Config config)
    {
        _configDbContext.Configs.Update(config);
        _configDbContext.SaveChanges();
    }

    /// <summary>
    /// Used by Admin to create a new config
    /// </summary>
    /// <param name="config"></param>
    public void AddConfig(Config config)
    {
        _configDbContext.Configs.Add(config);
        _configDbContext.SaveChanges();
    }
}