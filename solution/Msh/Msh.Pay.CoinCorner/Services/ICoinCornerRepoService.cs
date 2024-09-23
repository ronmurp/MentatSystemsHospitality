using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public interface ICoinCornerRepoService
{
	Task<CoinCornerConfig> GetConfig();

	Task SaveConfig(CoinCornerConfig config);

	Task<CoinCornerGlobal> GetGlobal();

	Task SaveGlobal(CoinCornerGlobal global);
}