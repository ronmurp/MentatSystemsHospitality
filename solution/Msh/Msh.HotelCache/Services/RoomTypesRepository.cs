using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;
public interface IRoomTypeRepository : IBaseHotelRepository<RoomType> { }

public class RoomTypeRepository(IConfigRepository configRepository)
	: AbstractHotelRepository(configRepository), IRoomTypeRepository
{
	public override string ConfigType(string hotelCode) => 
		HotelConfigType(ConstHotel.Cache.RoomTypes, hotelCode);

	public async Task<List<RoomType>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<RoomType>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<RoomType>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<RoomType>>(ConfigType(hotelCode));

	public async Task<List<RoomType>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<RoomType>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<RoomType> items, string hotelCode, string notes = "") =>
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);
}
