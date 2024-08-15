using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public interface ICoinCornerRepoService
{
	CoinCornerConfig GetConfig();

	void SaveConfig(CoinCornerConfig config);

	CoinCornerGlobal GetGlobal();

	void SaveGlobal(CoinCornerGlobal global);
}