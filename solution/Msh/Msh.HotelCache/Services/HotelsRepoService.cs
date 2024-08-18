using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public interface IHotelsRepoService
{
	List<Hotel> GetHotels();

	void SaveHotels(List<Hotel> hotels);
}

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public class HotelsRepoService(IConfigRepository configRepository) : IHotelsRepoService
{
	public List<Hotel> GetHotels() =>
		configRepository.GetConfigContent<List<Hotel>>(ConstHotel.Cache.Hotel);

	public void SaveHotels(List<Hotel> hotels)
	{
		configRepository.SaveConfig(ConstHotel.Cache.Hotel, hotels);
	}


	
}