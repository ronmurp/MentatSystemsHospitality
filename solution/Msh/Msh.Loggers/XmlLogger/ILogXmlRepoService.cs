namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Used in Admin to edit LogXmlConfig
/// </summary>
public interface ILogXmlRepoService
{
	Task<LogXmlConfig> GetConfig();

	Task SaveConfig(LogXmlConfig config);

}