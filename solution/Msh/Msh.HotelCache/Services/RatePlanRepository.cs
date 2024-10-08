﻿using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.RatePlans;

namespace Msh.HotelCache.Services;

public interface IRatePlanRepository : IBaseHotelRepository<RoomRatePlan> { }
public class RatePlanRepository(IConfigRepository configRepository) 
	: AbstractHotelRepository(configRepository), IRatePlanRepository
{
	public override string ConfigType(string hotelCode) =>
		HotelConfigType(ConstHotel.Cache.RatePlans, hotelCode);

	public async Task<List<RoomRatePlan>> Archived(string hotelCode, string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode), archiveCode);

	public async Task<List<RoomRatePlan>> Published(string hotelCode) =>
		await ConfigRepository.GetConfigPubContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode));

	public async Task<List<RoomRatePlan>> GetData(string hotelCode) =>
		await ConfigRepository.GetConfigContentAsync<List<RoomRatePlan>>(ConfigType(hotelCode)) ?? [];

	public async Task<bool> Save(List<RoomRatePlan> items, string hotelCode) => 
		await ConfigRepository.SaveConfigAsync(ConfigType(hotelCode), items);
}