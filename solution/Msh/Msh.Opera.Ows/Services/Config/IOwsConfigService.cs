using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Config;

/// <summary>
/// Config service for Opera Cloud OWS
/// </summary>
public interface IOwsConfigService
{
	OwsConfig OwsConfig { get; }
	List<OwsConfig> OwsConfigList { get; }
	void Reload();

	string PaymentCode(string paymentScheme);
}