using Msh.Common.Data;
using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public class CoinCornerRepoService(IConfigRepository configRepository) : ICoinCornerRepoService
{
	public async Task<CoinCornerConfig> GetConfig() => 
		await configRepository.GetConfigContentAsync<CoinCornerConfig>(ConstCc.CoinCornerConfig);

	public async Task SaveConfig(CoinCornerConfig config)
	{
		await configRepository.SaveConfigAsync(ConstCc.CoinCornerConfig, config);
	}

	public async Task<CoinCornerGlobal> GetGlobal() =>
		await configRepository.GetConfigContentAsync<CoinCornerGlobal>(ConstCc.CoinCornerGlobal);

	public async Task SaveGlobal(CoinCornerGlobal global)
	{
		await configRepository.SaveConfigAsync(ConstCc.CoinCornerGlobal, global);
	}
}