using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;

public interface IHotelsRepoService
{
	Task<List<Hotel>> GetHotelsAsync();

	Task SaveHotelsAsync(List<Hotel> hotels);

	Task<List<RoomType>> GetRoomTypesAsync(string hotelCode);

	Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode);

	Task<List<TestModel>> GetTestModelsAsync();

	Task SaveTestModelsAsync(List<TestModel> testModels);
}

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public class HotelsRepoService(IConfigRepository configRepository) : IHotelsRepoService
{
	public async Task<List<Hotel>> GetHotelsAsync() =>
		await configRepository.GetConfigContentAsync<List<Hotel>>(ConstHotel.Cache.Hotel);

	public async Task SaveHotelsAsync(List<Hotel> hotels)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Hotel, hotels);
	}

	public async Task<List<RoomType>> GetRoomTypesAsync(string hotelCode) => 
		await configRepository.GetConfigContentAsync<List<RoomType>>(ConstHotel.Cache.RoomTypes, hotelCode);

	public async Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.RoomTypes, hotelCode,roomTypes);
	}

	public async Task<List<TestModel>> GetTestModelsAsync() => 
		await configRepository.GetConfigContentAsync<List<TestModel>>(ConstHotel.Cache.TestModel) ?? [];

	public async Task SaveTestModelsAsync(List<TestModel> testModels)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.TestModel, testModels);
	}
}