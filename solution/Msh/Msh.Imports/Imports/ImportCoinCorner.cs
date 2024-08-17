using System.Text.Json;
using Msh.Common.Models.Configuration;
using Msh.HotelCache.Models;
using Msh.Pay.CoinCorner.Models;
using Msh.TestSupport;
using NUnit.Framework;

namespace Msh.Imports.Imports;

[TestFixture]
public class ImportCoinCorner
{
    /// <summary>
    /// Import basic Coin Corner Config
    /// </summary>
    [Test]
    public void ImportCoinCornerConfig()
    {
        var ccConfig = new CoinCornerConfig
        {
            UserId = "454440",
            PublicKey = "e642373c-97bb-4da3-b085-bc9d95fe0c81",
            Secret = new ConfigSecret
            {
                SecretSource = ConfigSecretSource.InEnvVar,
                Target = EnvironmentVariableTarget.Machine,
                Name = "CoinCorner_SECRET_LIVE",
                Secret = string.Empty
            },
            CheckoutUrl = "https://checkout.CoinCorner.com/api",
            SuccessRedirectUrl = "https://localhost:44364/api/bitcoin/success",
            FailRedirectUrl = "https://localhost:44364/api/bitcoin/fail",
            NotificationUrl = "https://localhost:44364/api/bitcoin/notify",
            LogEnable = true,
            LogFilePath = "Payments\\CoinCorner"
        };

        TestConfigUtilities.SaveConfig(ConstCc.CoinCornerConfig, ccConfig);
    }

    /// <summary>
    /// Import basic Coin Corner Global
    /// </summary>
    [Test]
    public void ImportCoinCornerGlobal()
    {
        var config = new CoinCornerGlobal
        {
            EnableActivities = false,
            EnableBedrooms = true,
            Retries = 3,
            RetryInterval = 2,
            SettleCurrency = "GBP",
            InvoiceCurrency = "GBP",
            OnePencePayment = false,
            EnableLightning = true,
            EnableOnChain = true,
            LightningLimit = 500,
            LightningLimitMessage = "Lightning payments are limited to {0} GBP",
            NotAvailableMessage = "Bitcoin is not available for this booking.",
            NotAvailableActivitiesMessage = "Bitcoin is not available for Activities bookings.",
            NotAvailableBookingsMessage = "Bitcoin is not available for bedroom bookings.",
            OnChainPendingWait = 30,
            OnChainInitialPrompt = "On Chain payments may take several minutes once activated by your device, please wait after scanning or posting ...",
            StopWaitingPrompt = "Waiting for payment to complete ... or provisional payment."
        };

        TestConfigUtilities.SaveConfig(ConstCc.CoinCornerGlobal, config);
    }

}