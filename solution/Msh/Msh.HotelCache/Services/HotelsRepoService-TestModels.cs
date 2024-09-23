using Msh.HotelCache.Models;
using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public partial class HotelsRepoService
{
	public async Task<List<TestModel>> GetTestModelsAsync() =>
		await configRepository.GetConfigContentAsync<List<TestModel>>(ConstHotel.Cache.TestModel) ?? [];

	public async Task SaveTestModelsAsync(List<TestModel> testModels)
	{
		await configRepository.SaveConfigAsync(ConstHotel.Cache.TestModel, testModels);
	}

}