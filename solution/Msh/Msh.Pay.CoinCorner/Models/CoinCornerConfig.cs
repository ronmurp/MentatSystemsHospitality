using Msh.Common.Models.Configuration;

namespace Msh.Pay.CoinCorner.Models;

/// <summary>
/// All must be lower-case.
/// </summary>
public class CoinCornerConfig() : ConfigBase(ConstCc.CoinCornerConfig)
{
    public string UserId { get; set; }
    public string PublicKey { get; set; }

    public ConfigSecret Secret { get; set; } = new ConfigSecret();

    /// <summary>
    /// No trailing /
    /// </summary>
    public string CheckoutUrl { get; set; } = "https://checkout.CoinCorner.com/api";

    public string SuccessRedirectUrl { get; set; }

    public string FailRedirectUrl { get; set; }

    public string NotificationUrl { get; set; }

    public bool LogEnable { get; set; }

    public string LogFilePath { get; set; }
}