using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Msh.Common.Models.Configuration;

namespace Msh.Pay.FreedomPay.Models.Configuration;

public class FpConfig() : ConfigBase(ConstFp.FpConfig)
{
    [Display(Name="Version")]
    public string Version { get; set; } = "v1.5";

    public ConfigSecret Secret { get; set; } = new ConfigSecret
    {
        Target = EnvironmentVariableTarget.Machine,
        Name = "",
        SecretSource = ConfigSecretSource.InEnvVar,
        Secret = string.Empty
    };

    [Display(Name = "Merchant ID")]
	public string MerchantId { get; set; } = "ENGLKE";

	[Display(Name = "EsKey")]
	public string EsKey { get; set; } = "R2Y2X9M3793T46JHJFHG44QM2XBK36FF";

	[Display(Name = "Store ID")]
	public string StoreId { get; set; } = "1512822973";

	[Display(Name = "Terminal ID")]
	public string TerminalId { get; set; } = "2514588972";

	/// <summary>
	/// WorkflowType is optional. Default is "Standard" or 1 (can be string or number).
	/// </summary>
	[Display(Name = "Workflow Type")] 
	public string? WorkflowType { get; set; } = "Standard";

	/// <summary>
	/// Hides legal text and checkboxes by setting Legal = null
	/// </summary>
	[Display(Name = "Hide Legal")] 
	public bool HideLegal { get; set; } = true;

	/// <summary>
	/// If legal is to be shown, this shows it without checkbox
	/// </summary>
	[Display(Name = "Hide Legal Checkbox")] 
	public bool HideLegalCheckbox { get; set; } = true;

	[Display(Name = "Legal Text Type Card")]
	public string? LegalTextTypeCard { get; set; } = ConstFp.LegalTextType.DynamicWithoutCheckbox;

	[Display(Name = "Legal Text Type Third Party")]
	public string? LegalTextTypeThirdParty { get; set; } = ConstFp.LegalTextType.DynamicThirdPartyWithoutCheckbox;


	[Display(Name = "3D Secure Enabled")]
	public bool ThreeDSecureEnabled { get; set; }

	[Display(Name = "3D Soft Decline Enabled")]
	public bool SoftDeclineEnabled { get; set; }

	[Display(Name = "Currency Alphabetic Code")]
	public string? CurrencyAlphabeticCode { get; set; } = "GBP";

	/// <summary>
	/// 3 digit numeric ISO 4217 currency code for the sale amount
	/// </summary>
	[Display(Name = "Currency Numeric Code")] 
	public string? CurrencyNumericCode { get; set; } = "826";

	// Todo - FreedomPay - Q - What are these
	[Display(Name = "Token Information Token Type")] 
	public string? TokenInformationTokenType { get; set; } = "6";

	[Display(Name = "Consumer Authentication Script")]
	public string? ConsumerAuthenticationScript { get; set; } = string.Empty;

	/// <summary>
	/// FreedomPay API ~/api/v1.5/controls/init
	/// </summary>
	[Display(Name = "FreedomPay Url Init")] 
	public string? FreedomPayUrlInit { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/controls/init";

	/// <summary>
	/// FreedomPay API ~/api/v1.5/payments
	/// </summary>
	[Display(Name = "FreedomPay Url Payments")] 
	public string? FreedomPayUrlPayments { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments";

	/// <summary>
	/// FreedomPay API ~/api/v1.5/payments/acknowledge
	/// </summary>
	[Display(Name = "FreedomPay Url Payment Ack")] 
	public string? FreedomPayUrlPaymentAck { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments/acknowledge";

	/// <summary>
	/// FreedomPay API ~/api/v1.5/payments/reverse
	/// </summary>
	[Display(Name = "FreedomPay Url Payment Reverse")] 
	public string? FreedomPayUrlReverse { get; set; } = "https://hpc.uat.freedompay.com/api/v1.5/payments/reverse";

	/// <summary>
	/// FreedomPay page HTML script tags.
	/// </summary>
	[Display(Name = "Page Scripts")]
	[Category("TextArea")]
	public string? PageScripts { get; set; } = string.Empty;

	/// <summary>
	/// FreedomPay page HTML script tags for Premier Core.
	/// </summary>
	[Display(Name = "Page Scripts Pc")]
	[Category("TextArea")]
	public string? PageScriptsPc { get; set; } = string.Empty;


	/// <summary>
	/// The partial path in the main logs location - see appSettings["LogPath"]
	/// </summary>
	[Display(Name = "Log File Path")] 
	public string LogFilePath { get; set; } = string.Empty;

	[Display(Name = "Log Enabled")]
	public bool LogEnable { get; set; }

	/// <summary>
	/// The maximum number of submission attempts allowed, irrespective of iframe instances, for the same reservationId
	/// </summary>
	[Display(Name = "Max Submission Attempts")] 
	public int MaxSubmissionAttempts { get; set; }

	[Display(Name = "Selling Middleware Name")]
	public string? SellingMiddlewareName { get; set; } = string.Empty;

	[Display(Name = "Selling Middleware Version")]
	public string? SellingMiddlewareVersion { get; set; } = string.Empty;

	[Display(Name = "Selling System Name")]
	public string? SellingSystemName { get; set; } = string.Empty;

	[Display(Name = "Selling System Version")]
	public string? SellingSystemVersion { get; set; } = string.Empty;

	[Display(Name = "Api Timeout Seconds")]
	public int ApiTimeoutSeconds { get; set; }

	[Display(Name = "Bad State Transition")]
	public string? BadStateTransition { get; set; } = string.Empty;

	// public PayPalConfig PayPalConfig { get; set; } = new PayPalConfig();
	/// <summary>
	/// 1 - Unrestricted - Can accept pending alternate payment methods (APMs) where the final decision happens later.
	/// Note: Merchants must create an Hpc.WebhookUrl for these instances and provide it to FreedomPay’s Boarding Team.
	/// When the payment has finally been accepted, FP will send a success notification to this URL informing the integrator
	/// that the payment has completed successfully.Refer to Appendix A for more information.
	///
	/// 2 - ImmediatePaymentRequired - Can only accept instant payments (this will return an error if an APM is used that is pending).
	/// </summary>
	[Display(Name = "PayPalPay Method Payee Preferred")] 
	public string? PayPalPayMethPayeePreferred { get; set; } = string.Empty;

	[Display(Name = "PayPalPay Shipping Preference")]
	public string PayPalShippingPreference { get; set; } = ConstFp.Paypal.Shipping.NoShipping;

	[Display(Name = "Avs Enabled")]
	public bool AvsEnabled { get; set; }

	[Display(Name = "Cvv Enabled")]
	public bool CvvEnabled { get; set; }

	[Display(Name = "Avs Whitelist")]
	public string? AvsWhitelist { get; set; } = string.Empty;

	[Display(Name = "Cvv Whitelist")]
	public string? CvvWhitelist { get; set; } = string.Empty;

	// public AvsCvvWhitelist AvsCvvWhitelist { get; set; } = new AvsCvvWhitelist();

}