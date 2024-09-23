using Msh.Common.Data;
using Msh.Common.Models.Configuration;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

/// <summary>
/// Used in Admin to edit CoinCornerConfig and CoinCornerGlobal
/// </summary>
public partial class HotelsRepoService(IConfigRepository configRepository) : IHotelsRepoService
{
	public async Task<List<Hotel>> GetHotelsAsync() =>
		await configRepository.GetConfigContentAsync<List<Hotel>>(ConstHotel.Cache.Hotel);
	public async Task<List<Hotel>> GetHotelsPublishAsync() =>
		await configRepository.GetConfigPubContentAsync<List<Hotel>>(ConstHotel.Cache.Hotel);

	public async Task<List<ConfigArchiveBase>?> GetHotelsArchiveListAsync() => 
		await configRepository.GetConfigArchiveListAsync(ConstHotel.Cache.Hotel);

	public async Task<List<Hotel>> GetHotelsArchiveAsync(string archiveCode) =>
		await configRepository.GetConfigArchiveContentAsync<List<Hotel>>(ConstHotel.Cache.Hotel, archiveCode);

	public async Task SaveHotelsAsync(List<Hotel> hotels)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.Hotel, hotels);
	}

	public async Task<bool> PublishHotelsAsync(string userId) => 
		await configRepository.PublishConfigAsync(ConstHotel.Cache.Hotel, userId);

	public async Task<bool> ArchiveHotelsAsync(string archiveCode, string userId) =>
		await configRepository.SaveConfigArchiveAsync(ConstHotel.Cache.Hotel, archiveCode, userId);

	public async Task<bool> SaveHotelsArchiveAsync(List<Hotel> hotels, string archiveCode, string userId) =>
		await configRepository.SaveConfigArchiveAsync(ConstHotel.Cache.Hotel, archiveCode, userId);
}