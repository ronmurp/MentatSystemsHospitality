using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public class HotelRepository(IConfigRepository configRepository) : AbstractRepository(configRepository), IHotelRepository
{
	public override string ConfigType() => ConstHotel.Cache.Hotel;

	public async Task<List<Hotel>> GetData() =>
		await ConfigRepository.GetConfigContentAsync<List<Hotel>>(ConfigType());

	public async Task<List<Hotel>> Published() =>
		await ConfigRepository.GetConfigPubContentAsync<List<Hotel>>(ConfigType());

	public async Task<List<Hotel>> Archived(string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<Hotel>>(ConfigType(), archiveCode);

	public async Task<bool> Save(List<Hotel> hotels, string notes = "") => 
		await ConfigRepository.SaveConfigAsync(ConfigType(), hotels, notes);
}