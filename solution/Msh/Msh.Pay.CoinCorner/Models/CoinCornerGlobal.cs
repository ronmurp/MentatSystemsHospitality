using Msh.Common.Models.Configuration;

namespace Msh.Pay.CoinCorner.Models;

/// <summary>
/// Coin Corner Global Data - as opposed to Config.
/// Global can be edited by admin, Config only by Super/Dev
/// </summary>
public class CoinCornerGlobal() : ConfigBase(ConstCc.CcGlobal)
{
    /// <summary>
    /// Determined whether or not the Lightning option is enabled in the popup
    /// </summary>
    public bool EnableLightning { get; set; }

    /// <summary>
    /// Determined whether or not the on-chain (Bitcoin) option is enabled in the popup
    /// </summary>
    public bool EnableOnChain { get; set; }

    public bool EnabledActivities { get; set; }

    public bool EnabledBedrooms { get; set; }

    /// <summary>
    /// Number of times to retry if the API fails
    /// </summary>
    public int Retries { get; set; } = 3;

    /// <summary>
    /// Number of seconds between retries
    /// </summary>
    public int RetryInterval { get; set; } = 5;

    public string SettleCurrency { get; set; } = "GBP";

    public string InvoiceCurrency { get; set; } = "GBP";

    /// <summary>
    /// What is the max amount WBS will allow the use of Lightning
    /// </summary>
    public decimal LightningLimit { get; set; } = 500M;

#if DEBUG
    public bool OnePencePayment { get; set; }
#endif

    public string LightningLimitMessage { get; set; } = string.Empty;
    public string NotAvailableMessage { get; set; } = string.Empty;
    public string NotAvailableActivitiesMessage { get; set; } = string.Empty;
    public string NotAvailableBookingsMessage { get; set; } = string.Empty;

    /// <summary>
    /// Number of minutes WBS should wait for a confirmation complete before accepting payment
    /// </summary>
    public int OnChainPendingWait { get; set; } = 30;

    /// <summary>
    /// Displayed when on-chain is selected
    /// </summary>
    public string OnChainInitialPrompt { get; set; } = string.Empty;

    public string StopWaitingPrompt { get; set; } = string.Empty;
}