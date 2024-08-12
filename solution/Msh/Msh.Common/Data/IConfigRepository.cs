using Msh.Common.Models.Configuration;

namespace Msh.Common.Data;

/// <summary>
/// A repository for Config models
/// </summary>
public interface IConfigRepository
{
    Config? GetConfig(string configType);
    void SaveConfig(Config config);

    void SaveConfig<T>(string configType, T value);

    void AddConfig(Config config);
}