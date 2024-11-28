using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;


/// <summary>
/// Repository for type RoomRatePlan
/// </summary>
/// <param name="configRepository"></param>
public class RatePlanRepository(IConfigRepository configRepository) 
	: AbstractHotelRepository(configRepository), IRatePlanRepository
{
	public override string ConfigType(string hotelCode) =>
		HotelConfigType(ConstHotel.Cache.RatePlans, hotelCode);

	public async Task<List<RoomRatePlan>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<RoomRatePlan>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode));

	public async Task<List<RoomRatePlan>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<RoomRatePlan> items, string hotelCode, string notes = "") => 
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);

	public Task<bool> ArchiveDelete(string hotelCode, string archiveCode, string userId)
	{
		throw new NotImplementedException();
	}
}