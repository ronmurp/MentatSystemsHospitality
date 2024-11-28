using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;

public class RatePlanSortRepository(IConfigRepository configRepository)
	: AbstractHotelRepository(configRepository), IRatePlanSortRepository
{
	public override string ConfigType(string hotelCode) =>
		HotelConfigType(ConstHotel.Cache.RatePlanSort, hotelCode);

	public async Task<List<RatePlanSort>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<RatePlanSort>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<RatePlanSort>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<RatePlanSort>>(ConfigType(hotelCode));

	public async Task<List<RatePlanSort>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<RatePlanSort>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<RatePlanSort> items, string hotelCode, string notes = "") =>
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);

	public Task<bool> ArchiveDelete(string hotelCode, string archiveCode, string userId)
	{
		throw new NotImplementedException();
	}
}