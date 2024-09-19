using Msh.Opera.Ows.Models;

namespace Msh.Opera.Ows.Services.Config;

/// <summary>
/// Config service for Opera Cloud OWS
/// </summary>
public interface IOwsConfigService
{
	Task<OwsConfig> GetOwsConfigAsync();

	Task SaveHotelsAsync(OwsConfig config);

	OwsConfig OwsConfig { get; }

	string PaymentCode(string paymentScheme);
}