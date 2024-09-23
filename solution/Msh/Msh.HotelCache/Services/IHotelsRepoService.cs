using Msh.Common.Models.Configuration;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{
	Task<List<Hotel>> GetHotelsAsync();

	Task<List<Hotel>> GetHotelsPublishAsync();

	Task<List<ConfigArchiveBase>?> GetHotelsArchiveListAsync();

	Task<List<Hotel>> GetHotelsArchiveAsync(string archiveCode);

	Task SaveHotelsAsync(List<Hotel> hotels);

	Task<bool> PublishHotelsAsync(string userId);

	Task<bool> ArchiveHotelsAsync(string archiveCode, string userId);

	Task<bool> SaveHotelsArchiveAsync(List<Hotel> hotels, string archiveCode, string userId);

	Task<bool> LockPubHotelsAsync(bool performLock,string userId);

	Task<bool> LockArchiveHotelsAsync(string archiveCode, bool performLock, string userId);
}