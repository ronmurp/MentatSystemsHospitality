using System.Text.RegularExpressions;
using System.Xml.Linq;
using Msh.Common.Data;
using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Config;

/// <summary>
/// Config service for Opera Cloud OWS
/// </summary>
public class OwsConfigService(IConfigRepository configRepository) : IOwsConfigService
{
	public async Task<OwsConfig> GetOwsConfigAsync() => 
		await configRepository.GetConfigContentAsync<OwsConfig>(OwsConst.Cache.OwsConfig);

	public async Task SaveHotelsAsync(OwsConfig config)
	{
		await configRepository.SaveConfigAsync(OwsConst.Cache.OwsConfig, config);
	}

	/// <summary>
	/// Use by default - depends on RunMode of system
	/// </summary>
	public OwsConfig OwsConfig
	{
		get
		{
			return new OwsConfig();
		}
	}

	/// <summary>
	/// A list that can be used on test systems to call UAT or production OWS
	/// </summary>
	public List<OwsConfig> OwsConfigList => _owsConfigs ?? (_owsConfigs = LoadConfig());

	/// <summary>
	/// Used in test environments to force reload after updating values. No concurrency lock, but only used for tests.
	/// </summary>
	public void Reload()
	{
		_owsConfigs = null;
	}

	public string PaymentCode(string paymentScheme)
	{
		var config = OwsConfig;

		// Don't want a null in a regex. At least use the default
		var scheme = paymentScheme ?? string.Empty;

		var list = config.SchemeMap;
		foreach (var map in list)
		{
			if (Regex.IsMatch(scheme, map.Pattern))
			{
				return map.Code;
			}
		}
		//WbsLogger.Error(LogCodes.OwsSchemeMap, new Exception($"Map for scheme {paymentScheme} not found in OperaCloudConfig.xml. Using default {config.DefaultCardPaymentMethod}"));
		return config.DefaultCardPaymentMethod;
	}

	private static List<OwsConfig> _owsConfigs;

	private List<OwsConfig> LoadConfig()
	{
		//var filename = Path.Combine(configService.AppDataPath, "OperaCloudConfig", "OperaCloudConfig.xml");

		//var xdoc = XDocument.Load(filename);

		//var list = xdoc.Descendants("Site")
		//	.Select(c => new OwsConfig
		//	{
				
		//		//ElhUserId = c.ValueE("ElhUserId"),
		//		//Password = c.ValueE("Password"),
		//		//PasswordEnvVar = c.ValueE("PasswordEnvVar"),
		//		//ChainCode = c.ValueE("ChainCode"),
		//		//DefaultHotelCode = c.ValueE("LWH", "DefaultHotelCode"),
		//		//BaseUrl = c.ValueE("BaseUrl"),
		//		//RetryCount = c.ValueE(3, "Retries"),
		//		//CriticalErrorTriggers = c.Descendants("CriticalErrorTriggers").Descendants("Text")
		//		//	.Select(e => new CriticalErrorTrigger
		//		//	{
		//		//		Code = e.ValueA("code"),
		//		//		Trigger = e.ValueE(),
		//		//		ErrorType = e.ValueA("Critical", "errorType").Get<CriticalErrorType>(),
		//		//		IsRegEx = e.ValueA(false, "isRegex")
		//		//	}).ToList(),
		//		//DefaultCardPaymentMethod = c.ValueE("WEBMC","CardPaymentMethodDefault"),
		//		//SchemeMap = c.Descendants("SchemeMap").Descendants("Scheme")
		//		//	.Select(m => new OwsPaymentCodeMap
		//		//	{
		//		//		Code = m.ValueA("code"),
		//		//		Pattern = m.ValueE()
		//		//	}).ToList(),
		//		//VoucherPaymentMethod = c.ValueE("CRSGVS", "VoucherPaymentMethod")
		//	}).ToList();

		//foreach (var config in list)
		//{
		//	if (!string.IsNullOrEmpty(config.PasswordEnvVar))
		//	{
		//		config.Password = _configService.GetEnvironmentVariable(config.PasswordEnvVar, EnvironmentVariableTarget.Machine, config.Password);
		//	}
		//}

		//return list;

		return new List<OwsConfig>();
	}
}