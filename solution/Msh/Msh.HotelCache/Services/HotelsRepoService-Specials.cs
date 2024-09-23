using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Specials;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{
	public async Task<List<Special>> GetSpecialsAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<Special>>(ConstHotel.Cache.Specials, hotelCode) ?? [];


	public async Task SaveSpecialsAsync(List<Special> specials, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Specials, hotelCode, specials);
	}

}