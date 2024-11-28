using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Specials;

namespace Msh.HotelCache.Services;

public interface ISpecialsRepository: IBaseHotelRepository<Special> { }

public class SpecialsRepository(IConfigRepository configRepository): AbstractHotelRepository(configRepository), ISpecialsRepository
{
	public override string ConfigType(string hotelCode) => HotelConfigType(ConstHotel.Cache.Specials, hotelCode);

	public async Task<List<Special>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<Special>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<Special>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<Special>>(ConfigType(hotelCode));

	public async Task<List<Special>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<Special>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<Special> items, string hotelCode, string notes = "") =>
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);
}