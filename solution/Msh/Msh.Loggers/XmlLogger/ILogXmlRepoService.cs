namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Used in Admin to edit LogXmlConfig
/// </summary>
public interface ILogXmlRepoService
{
	Task<LogXmlConfig> GetConfig(string group);

	Task SaveConfig(LogXmlConfig config, string group);

}