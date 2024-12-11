using Msh.Common.Constants;
using Msh.Common.Data;
using Msh.Common.Models.Captcha;
using Msh.Common.Models.Configuration;

namespace Msh.Common.Services;

/// <summary>
/// A place to record various states that other records can be recorded in.
/// </summary>
public class CaptchaConfigRepoService(IConfigRepository configRepository) : AbstractRepository(configRepository), ICaptchaConfigRepoService
{
	public override string ConfigType() => RepoConst.CaptchaConfig;

	public Task<ConfigArchiveBase?> Archived()
	{
		throw new NotImplementedException();
	}

	public async Task<CaptchaConfig> Archived(string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<CaptchaConfig>(ConfigType(), archiveCode);


	public async Task<CaptchaConfig> Published() =>
		await ConfigRepository.GetConfigPubContentAsync<CaptchaConfig>(ConfigType());

	public async Task<CaptchaConfig> GetData() =>
		await ConfigRepository.GetConfigContentAsync<CaptchaConfig>(ConfigType());

	public async Task<bool> Save(CaptchaConfig config, string notes = "") =>
		await ConfigRepository.SaveConfigAsync(ConfigType(), config, notes);

	public async Task<bool> ArchiveDelete(string archiveCode, string userId) =>
		await ConfigRepository.DeleteArchiveConfigAsync(ConfigType(), archiveCode, userId);
}