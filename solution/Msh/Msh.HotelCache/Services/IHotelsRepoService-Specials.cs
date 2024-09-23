using Msh.HotelCache.Models.Specials;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{

	Task<List<Special>> GetSpecialsAsync(string hotelCode);

	Task SaveSpecialsAsync(List<Special> specials, string hotelCode);

}