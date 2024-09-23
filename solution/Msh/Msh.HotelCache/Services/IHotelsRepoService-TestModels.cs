using Msh.HotelCache.Models.Hotels;

namespace Msh.HotelCache.Services;

public partial interface IHotelsRepoService
{
	Task<List<TestModel>> GetTestModelsAsync();

	Task SaveTestModelsAsync(List<TestModel> testModels);

}