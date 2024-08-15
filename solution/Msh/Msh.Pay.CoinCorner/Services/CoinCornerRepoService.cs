using Msh.Common.Data;
using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public class CoinCornerRepoService(IConfigRepository configRepository) : ICoinCornerRepoService
{
	public CoinCornerConfig GetConfig() => 
		configRepository.GetConfigContent<CoinCornerConfig>(ConstCc.CoinCornerConfig);

	public void SaveConfig(CoinCornerConfig config)
	{
		configRepository.SaveConfig(ConstCc.CoinCornerConfig, config);
	}

	public CoinCornerGlobal GetGlobal() =>
		configRepository.GetConfigContent<CoinCornerGlobal>(ConstCc.CoinCornerGlobal);

	public void SaveGlobal(CoinCornerGlobal global)
	{
		configRepository.SaveConfig(ConstCc.CoinCornerGlobal, global);
	}
}