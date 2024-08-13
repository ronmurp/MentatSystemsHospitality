using Msh.Common.Models.Configuration;

namespace Msh.Pay.FreedomPay.Models.Configuration;

public class FpConfig() : ConfigBase(ConstFp.FpConfig)
{
    public string Version { get; set; } = "v1.5";

    public ConfigSecret Secret { get; set; } = new ConfigSecret
    {
        Target = EnvironmentVariableTarget.Machine,
        Name = "",
        SecretSource = ConfigSecretSource.InEnvVar,
        Secret = string.Empty
    };

    public string MerchantId { get; set; } = "ENGLKE";

    public string EsKey { get; set; } = "R2Y2X9M3793T46JHJFHG44QM2XBK36FF";
    public string StoreId { get; set; } = "1512822973";
    public string TerminalId { get; set; } = "2514588972";

    /// <summary>
    /// WorkflowType is optional. Default is "Standard" or 1 (can be string or number).
    /// </summary>
    public string WorkflowType { get; set; } = "Standard";

    /// <summary>
    /// Hides legal text and checkboxes by setting Legal = null
    /// </summary>
    public bool HideLegal { get; set; } = true;

    /// <summary>
    /// If legal is to be shown, this shows it without checkbox
    /// </summary>
    public bool HideLegalCheckbox { get; set; } = true;
    public string LegalTextTypeCard { get; set; } = ConstFp.LegalTextType.DynamicWithoutCheckbox;
    public string LegalTextTypeThirdParty { get; set; } = ConstFp.LegalTextType.DynamicThirdPartyWithoutCheckbox;

    public ThreeDSecure ThreeDSecure { get; set; } = new ThreeDSecure
    {
        Enabled = false,

        SoftDeclineEnabled = false
    };

    public string CurrencyAlphabeticCode { get; set; } = "GBP";

    /// <summary>
    /// 3 digit numeric ISO 4217 currency code for the sale amount
    /// </summary>
    public string CurrencyNumericCode { get; set; } = "826";

    // Todo - FreedomPay - Q - What are these
    public string TokenInformationTokenType { get; set; } = "6";
    public string ConsumerAuthenticationScript { get; set; } = string.Empty;

    /// <summary>
    /// FreedomPay API ~/api/v1.5/controls/init
    /// </summary>
    public string FreedomPayUrlInit { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/controls/init";

    /// <summary>
    /// FreedomPay API ~/api/v1.5/payments
    /// </summary>
    public string FreedomPayUrlPayments { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments";

    /// <summary>
    /// FreedomPay API ~/api/v1.5/payments/acknowledge
    /// </summary>
    public string FreedomPayUrlPaymentAck { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments/acknowledge";

    /// <summary>
    /// FreedomPay API ~/api/v1.5/payments/reverse
    /// </summary>
    public string FreedomPayUrlReverse { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments/reverse";

    /// <summary>
    /// FreedomPay page HTML script tags.
    /// </summary>
    public string PageScripts { get; set; } = string.Empty;

    /// <summary>
    /// FreedomPay page HTML script tags for Premier Core.
    /// </summary>
    public string PageScriptsPc { get; set; } = string.Empty;


    /// <summary>
    /// The partial path in the main logs location - see appSettings["LogPath"]
    /// </summary>
    public string LogFilePath { get; set; } = string.Empty;

    public bool LogEnable { get; set; }

    /// <summary>
    /// The maximum number of submission attempts allowed, irrespective of iframe instances, for the same reservationId
    /// </summary>
    public int MaxSubmissionAttempts { get; set; }

    public string SellingMiddlewareName { get; set; } = string.Empty;
    public string SellingMiddlewareVersion { get; set; } = string.Empty;
    public string SellingSystemName { get; set; } = string.Empty;
    public string SellingSystemVersion { get; set; } = string.Empty;
    public int ApiTimeoutSeconds { get; set; }

    public string BadStateTransition { get; set; } = string.Empty;

    public PayPalConfig PayPalConfig { get; set; } = new PayPalConfig();

    public AvsCvvWhitelist AvsCvvWhitelist { get; set; } = new AvsCvvWhitelist();

}