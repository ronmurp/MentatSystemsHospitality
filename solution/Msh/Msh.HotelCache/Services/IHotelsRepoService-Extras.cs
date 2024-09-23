using Msh.HotelCache.Models.Extras;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{

	Task<List<Extra>> GetExtrasAsync(string hotelCode);

	Task SaveExtrasAsync(List<Extra> extras, string hotelCode);

}