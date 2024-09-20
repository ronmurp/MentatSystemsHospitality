using Msh.Common.Data;
using Msh.Loggers.Const;

namespace Msh.Loggers.XmlLogger;

/// <summary>
/// Used in Admin to edit LogXmlConfig
/// </summary>
public class LogXmlRepoService(IConfigRepository configRepository) : ILogXmlRepoService
{

	public async Task<LogXmlConfig> GetConfig(string group) =>
		await configRepository.GetConfigContentAsync<LogXmlConfig>($"{ConstLog.LogXmlConfig}-{group}");

	public async Task SaveConfig(LogXmlConfig config, string group)
	{
		await configRepository.SaveConfigAsync($"{ConstLog.LogXmlConfig}-{group}", config);
	}
}