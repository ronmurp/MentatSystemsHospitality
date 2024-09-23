using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{
	public async Task<List<Extra>> GetExtrasAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<Extra>>(ConstHotel.Cache.Extras, hotelCode) ?? [];

	public async Task SaveExtrasAsync(List<Extra> extras, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Extras, hotelCode, extras);
	}

}