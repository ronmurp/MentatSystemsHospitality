namespace Msh.Common.Models.Configuration;

/// <summary>
/// Abstract base class for config data that is to
/// be stored in the Configs database table
/// </summary>
/// <param name="configType"></param>
public abstract class ConfigBase(string configType)
{
    /// <summary>
    /// Identify the Configs ConfigType name as the key for the type of data
    /// </summary>
    public string ConfigType { get; private set; } = configType;
}