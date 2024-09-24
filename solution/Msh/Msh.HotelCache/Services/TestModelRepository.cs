using Msh.Common.Data;
using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public interface ITestModelRepository : IBaseConfigRepository<List<TestModel>> { }

public class TestModelRepository(IConfigRepository configRepository) 
	: AbstractRepository(configRepository), ITestModelRepository
{
	public override string ConfigType() =>
		ConstHotel.Cache.TestModel;

	public async Task<List<TestModel>> GetData() =>
		await ConfigRepository.GetConfigContentAsync<List<TestModel>>(ConfigType());

	public async Task<List<TestModel>> Archived(string archiveCode) =>
		await ConfigRepository.GetConfigArchiveContentAsync<List<TestModel>>(ConfigType(), archiveCode);

	public async Task<List<TestModel>> Published() =>
		await ConfigRepository.GetConfigPubContentAsync<List<TestModel>>(ConfigType());

	
	public async Task<bool> Save(List<TestModel> items) =>
		await ConfigRepository.SaveConfigAsync(ConfigType(), items);
}