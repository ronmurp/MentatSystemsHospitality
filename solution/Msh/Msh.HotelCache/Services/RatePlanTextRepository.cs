using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;


/// <summary>
/// Repository for type RatePlanText
/// </summary>
/// <param name="configRepository"></param>
public class RatePlanTextRepository(IConfigRepository configRepository)
	: AbstractHotelRepository(configRepository), IRatePlanTextRepository
{
	public override string ConfigType(string hotelCode) =>
		HotelConfigType(ConstHotel.Cache.RatePlansText, hotelCode);

	public async Task<List<RatePlanText>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<RatePlanText>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<RatePlanText>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<RatePlanText>>(ConfigType(hotelCode));

	public async Task<List<RatePlanText>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<RatePlanText>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<RatePlanText> items, string hotelCode) =>
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items);
}