using System.ComponentModel.DataAnnotations;
using Msh.Common.Exceptions;

namespace Msh.Opera.Ows.Models;

/// <summary>
/// Live or test
/// </summary>
public class OwsConfig
{
	// Todo - OwsConfig - Get from appSettings
	/// <summary>
	/// The channel user Id
	/// </summary>
	[Display(Name="ELH User ID (Channel User ID)")]
	public string ElhUserId { get; set; } = "ELH_OWS_WBS";

	// Todo - OwsConfig - Get from envirnoment variable
	/// <summary>
	/// Password for Ows messages
	/// </summary>
	[Display(Name = "Password")] 
	public string? Password { get; set; } = "svPfbE5Z6E8?dUXEIjufCaQjP";

	[Display(Name = "Chain Code")]
	public string? ChainCode { get; set; } = "ELH";

	/// <summary>
	/// The Opera Cloud security requires a hotelCode, but not all Opera messages need one. Use a default.
	/// </summary>
	[Display(Name = "Default Hotel Code")] 
	public string? DefaultHotelCode { get; set; } = "LWH";

	[Display(Name = "Password Environment Variable")]
	public string? PasswordEnvVar { get; set; } = string.Empty;

	// Todo - OwsConfig - Get from appSettings
	/// <summary>
	/// 
	/// </summary>
	[Display(Name = "BASE URL")]
	public string BaseUrl { get; set; } =
		"https://he13-uat-ssd-osb.oracleindustry.com:443/OPERAOSB/OPERA_OWS/OWS_WS_51/";
       
	

	/// <summary>
	/// The number of retries a post will attempt before giving up
	/// </summary>
	[Display(Name = "OWS Retry Count")] 
	public int RetryCount { get; set; } = 3;

	/// <summary>
	///  List of triggers in content response that mark a critical error
	/// </summary>
	
	public List<CriticalErrorTrigger> CriticalErrorTriggers { get; set; } = [];

	/// <summary>
	/// Maps scheme to payment code
	/// </summary>
	public List<OwsPaymentCodeMap> SchemeMap { get; set; } = [];

	/// <summary>
	/// The method code to use if the Scheme map doesn't have a value
	/// </summary>
	[Display(Name = "Default Card Payment Method")] 
	public string DefaultCardPaymentMethod { get; set; } = "WEBMC";

	/// <summary>
	/// The code to send to Opera for voucher payments
	/// </summary>
	[Display(Name = "Voucher Paymen tMethod")] 
	public string VoucherPaymentMethod { get; set; } = "CRSGVS";
}