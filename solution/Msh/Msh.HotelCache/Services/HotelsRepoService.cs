using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;

public interface IHotelsRepoService
{
	List<Hotel> GetHotels();

	void SaveHotels(List<Hotel> hotels);

	List<RoomType> GetRoomTypes(string hotelCode);

	void SaveRoomTypes(List<RoomType> roomTypes, string hotelCode);
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

	public List<RoomType> GetRoomTypes(string hotelCode) => 
		configRepository.GetConfigContent<List<RoomType>>(ConstHotel.Cache.RoomTypes, hotelCode);

	public void SaveRoomTypes(List<RoomType> roomTypes, string hotelCode)
	{
		configRepository.SaveConfig(ConstHotel.Cache.RoomTypes, hotelCode,roomTypes);
	}
}