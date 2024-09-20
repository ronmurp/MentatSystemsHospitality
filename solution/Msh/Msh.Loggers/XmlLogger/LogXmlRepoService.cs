using Msh.Common.Data;
using Msh.Loggers.Const;

namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Used in Admin to edit LogXmlConfig
/// </summary>
public class LogXmlRepoService(IConfigRepository configRepository) : ILogXmlRepoService
{

	public async Task<LogXmlConfig> GetConfig() =>
		await configRepository.GetConfigContentAsync<LogXmlConfig>(ConstLog.LogXmlConfig);

	public async Task SaveConfig(LogXmlConfig config)
	{
		await configRepository.SaveConfigAsync(ConstLog.LogXmlConfig, config);
	}
}