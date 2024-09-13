﻿using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Extras;
using Msh.HotelCache.Models.Hotels;
using Msh.HotelCache.Models.RatePlans;
using Msh.HotelCache.Models.RoomTypes;

namespace Msh.HotelCache.Services;

public interface IHotelsRepoService
{
	Task<List<Hotel>> GetHotelsAsync();

	Task SaveHotelsAsync(List<Hotel> hotels);

	Task<List<RoomType>> GetRoomTypesAsync(string hotelCode);

	Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode);

	Task<List<RoomRatePlan>> GetRatePlansAsync(string hotelCode);

	Task SaveRatePlansAsync(List<RoomRatePlan> ratePlans, string hotelCode);

	Task<List<Extra>> GetExtrasAsync(string hotelCode);

	Task SaveExtrasAsync(List<Extra> extras, string hotelCode);

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
		await configRepository.GetConfigContentAsync<List<RoomType>>(ConstHotel.Cache.RoomTypes, hotelCode) ?? [];

	public async Task SaveRoomTypesAsync(List<RoomType> roomTypes, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.RoomTypes, hotelCode,roomTypes);
	}

	public async Task<List<RoomRatePlan>> GetRatePlansAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<RoomRatePlan>>(ConstHotel.Cache.RatePlans, hotelCode) ?? [];


	public async Task SaveRatePlansAsync(List<RoomRatePlan> ratePlans, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.RatePlans, hotelCode, ratePlans);
	}

	public async Task<List<Extra>> GetExtrasAsync(string hotelCode) =>
		await configRepository.GetConfigContentAsync<List<Extra>>(ConstHotel.Cache.Extras, hotelCode) ?? [];


	public async Task SaveExtrasAsync(List<Extra> extras, string hotelCode)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Extras, hotelCode, extras);
	}

	public async Task<List<TestModel>> GetTestModelsAsync() => 
		await configRepository.GetConfigContentAsync<List<TestModel>>(ConstHotel.Cache.TestModel) ?? [];

	public async Task SaveTestModelsAsync(List<TestModel> testModels)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.TestModel, testModels);
	}
}