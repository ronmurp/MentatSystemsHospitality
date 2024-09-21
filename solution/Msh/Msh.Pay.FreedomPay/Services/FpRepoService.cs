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

}