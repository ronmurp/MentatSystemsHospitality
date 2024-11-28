using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;

namespace Msh.HotelCache.Services;

public interface IExtraRepository :IBaseHotelRepository<Extra> { }

public class ExtraRepository(IConfigRepository configRepository)
	: AbstractHotelRepository(configRepository), IExtraRepository
{
	public override string ConfigType(string hotelCode) =>
		HotelConfigType(ConstHotel.Cache.Extras, hotelCode);

	public async Task<List<Extra>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<Extra>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<Extra>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<Extra>>(ConfigType(hotelCode));

	public async Task<List<Extra>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<Extra>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<Extra> items, string hotelCode, string notes = "") =>
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);

	public async Task<bool> ArchiveDelete(string hotelCode, string archiveCode, string userId) =>
		await ConfigRepository.DeleteArchiveConfigAsync(ConfigType(hotelCode), archiveCode, userId);

}