using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Discounts;

namespace Msh.HotelCache.Services;

public interface IDiscountRepository : IBaseHotelRepository<DiscountCode> { }
public class DiscountRepository(IConfigRepository configRepository) : 
	AbstractHotelRepository(configRepository), IDiscountRepository
{
	public override string ConfigType(string hotelCode) => HotelConfigType(ConstHotel.Cache.Discounts, hotelCode);
	
	public async Task<List<DiscountCode>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<DiscountCode>>(ConfigType(hotelCode), archiveCode);
	public async Task<List<DiscountCode>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<DiscountCode>>(ConfigType(hotelCode));
	public async Task<List<DiscountCode>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<DiscountCode>>(ConfigType(hotelCode)) ?? [];

	
	public async Task<bool> Save(List<DiscountCode> items, string hotelCode, string notes = "") => 
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items, notes);

	public Task<bool> ArchiveDelete(string hotelCode, string archiveCode, string userId)
	{
		throw new NotImplementedException();
	}
}