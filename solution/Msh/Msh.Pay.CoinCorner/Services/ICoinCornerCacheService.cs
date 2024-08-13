using Msh.Pay.CoinCorner.Models;

namespace Msh.Pay.CoinCorner.Services;

/// <summary>
/// Return (or reload) CoinCornerConfig & CoinCornerGlobal
/// </summary>
public interface ICoinCornerCacheService
{
    Task<CoinCornerConfig> GetCcConfig();

    Task<CoinCornerGlobal> GetCcGlobal();

    void ReloadConfig();

    void ReloadGlobal();
}