using Msh.Common.Data;
using Msh.Pay.FreedomPay.Models;
using Msh.Pay.FreedomPay.Models.Configuration;

namespace Msh.Pay.FreedomPay.Services;

public class FpRepoService(IConfigRepository configRepository) : IFpRepoService
{
	public async Task<FpConfig> GetFpConfig() =>
		await configRepository.GetConfigContentAsync<FpConfig>($"{ConstFp.FpConfig}");

	public async Task SaveFpConfig(FpConfig config)
	{
		await configRepository.SaveConfigAsync($"{ConstFp.FpConfig}", config);
	}

	public async Task<List<FpErrorCodeBank>> GetFpErrorCodeBank() =>
		await configRepository.GetConfigContentAsync<List<FpErrorCodeBank>>($"{ConstFp.FpErrorCodeBank}");

	public async Task SaveFpErrorCodeBank(List<FpErrorCodeBank> list)
	{
		await configRepository.SaveConfigAsync($"{ConstFp.FpErrorCodeBank}", list);
	}
}